using System.Media;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using offlineMeeting.Models.JsonDataProperty;

namespace offlineMeeting.Models.Process.QR
{
    /// <summary>
    /// ローカルで起動している VOICEVOX ENGINE (既定: http://127.0.0.1:50021) を叩いて
    /// 音声合成・再生を行うクラス。
    /// query 等の設定は VoiceVoxSettings_*.json で別ファイル管理する。
    /// メッセージ内容は仮置き（後で差し替え予定）。
    /// </summary>
    public class VoiceVoxProcess
    {
        // HttpClient はソケット枯渇を避けるため使い回す
        private static readonly HttpClient _httpClient = new();

        private readonly VoiceVoxSettingsProperty _settings;

        public VoiceVoxProcess()
        {
            _settings = new VoiceVoxSettingsProperty();
        }

        /// <summary>
        /// 指定した名前を使って音声を合成・再生する。
        /// 音声は補助機能のため、失敗しても例外を投げず本処理に影響させない。
        /// </summary>
        /// <param name="message">読み上げ対象のメッセージ（任意）</param>
        public async Task Execute(string message)
        {
            try
            {
                var query = await CreateAudioQuery(message);
                if (query is null)
                {
                    return;
                }

                ApplySettings(query);

                byte[]? wav = await Synthesis(query);
                if (wav is null)
                {
                    return;
                }

                Play(wav);
            }
            catch
            {
                // 音声合成・再生の失敗は無視する
            }
        }

        /// <summary>
        /// /audio_query を呼び出して合成用クエリを取得する。
        /// </summary>
        private async Task<JsonObject?> CreateAudioQuery(string text)
        {
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(_settings.TimeoutSeconds));

            var url = $"{_settings.BaseUrl}/audio_query" +
                      $"?text={Uri.EscapeDataString(text)}" +
                      $"&speaker={_settings.Speaker}";

            using var response = await _httpClient.PostAsync(url, null, cts.Token);
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var json = await response.Content.ReadAsStringAsync(cts.Token);
            return JsonNode.Parse(json) as JsonObject;
        }

        /// <summary>
        /// 別ファイル管理している query 設定を合成クエリへ反映する。
        /// </summary>
        private void ApplySettings(JsonObject query)
        {
            query["speedScale"] = _settings.SpeedScale;
            query["pitchScale"] = _settings.PitchScale;
            query["intonationScale"] = _settings.IntonationScale;
            query["volumeScale"] = _settings.VolumeScale;
            query["prePhonemeLength"] = _settings.PrePhonemeLength;
            query["postPhonemeLength"] = _settings.PostPhonemeLength;
        }

        /// <summary>
        /// /synthesis を呼び出して wav データを取得する。
        /// </summary>
        private async Task<byte[]?> Synthesis(JsonObject query)
        {
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(_settings.TimeoutSeconds));

            var url = $"{_settings.BaseUrl}/synthesis?speaker={_settings.Speaker}";

            using var content = new StringContent(query.ToJsonString(), Encoding.UTF8, "application/json");
            using var response = await _httpClient.PostAsync(url, content, cts.Token);
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return await response.Content.ReadAsByteArrayAsync(cts.Token);
        }

        /// <summary>
        /// 合成した wav をローカル（本アプリが動作しているPC）で再生する。
        /// </summary>
        private static void Play(byte[] wav)
        {
            if (!OperatingSystem.IsWindows())
            {
                return;
            }

            using var ms = new MemoryStream(wav);
            using var player = new SoundPlayer(ms);
            player.PlaySync();
        }
    }
}
