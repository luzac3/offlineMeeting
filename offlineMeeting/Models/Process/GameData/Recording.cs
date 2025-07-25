using offlineMeeting.Models.Entity.GameData;
using offlineMeeting.Models.Entity.Share;
using offlineMeeting.Models.Hubs;
using offlineMeeting.Models.Process.Share;
using Microsoft.AspNetCore.SignalR;

namespace offlineMeeting.Models.Process.GameData
{
    public class Recording
    {
        public async void StartRecording(
            RecordedMediaPostEntity recordedMediaPostEntity,
            IHubContext<SignalRHub> hubcontext,
            string targetPath,
            string targetClientName
        )
        {
            string baseTargetPath =
                recordedMediaPostEntity.EventNumber.ToString() + "_" +
                recordedMediaPostEntity.GameNumber.ToString() + "/" +
                targetPath;

            string numberPath =
                recordedMediaPostEntity.HandNumber.ToString() + "_" +
                recordedMediaPostEntity.HandSubNumber.ToString() + "/";

            int? mediaNumber = recordedMediaPostEntity.MediaNumber;
            bool isVideo = recordedMediaPostEntity.IsVideo;

            string baseDirectory =
                EnvironmentEntity.ContentRootPath +
                $@"/wwwroot/media/" +
                baseTargetPath +
                "now/" +
                (mediaNumber != null ? mediaNumber.ToString() : "0") + "/";

            string tsFileDirectory = baseDirectory + "TS/" + (isVideo ? "VIDEO/" : "AUDIO/");

            string tsNumberFileDirectory = tsFileDirectory + numberPath;
            string webmNumberFileDirectory =
                baseDirectory + 
                "WEBM/" + 
                (isVideo ? "video/" : "audio/") + 
                numberPath;

            RecordMediaProcess recordMediaProcess = new RecordMediaProcess();
            await recordMediaProcess.RecordMedia(
                recordedMediaPostEntity.EventNumber,
                recordedMediaPostEntity.GameNumber,
                recordedMediaPostEntity.HandNumber,
                recordedMediaPostEntity.HandSubNumber,
                mediaNumber,
                recordedMediaPostEntity.MediaCount,
                recordedMediaPostEntity.MediaMinutes,
                recordedMediaPostEntity.Base64 ?? throw new Exception("base64 empty"),
                isVideo,
                hubcontext,
                numberPath,
                tsFileDirectory,
                tsNumberFileDirectory,
                webmNumberFileDirectory,
                targetClientName
            );

            if (recordedMediaPostEntity.IsChangeFile)
            {
                string finishedFileDirectory =
                    EnvironmentEntity.ContentRootPath +
                    $@"/wwwroot/media/" +
                    baseTargetPath +
                    "finished/" +
                    (mediaNumber != null ? mediaNumber.ToString() : "0") + "/" +
                    (isVideo ? "VIDEO/" : "AUDIO/");

                string m3u8FilePath = tsFileDirectory + "index.m3u8";

                ChangeRecording(
                    hubcontext,
                    finishedFileDirectory,
                    tsNumberFileDirectory,
                    webmNumberFileDirectory,
                    numberPath,
                    m3u8FilePath
                );
            }
        }
        public async Task FinishRecording(
            string eventNumber,
            string gameSubNumber,
            string targetPath,
            IHubCallerClients Clients
        )
        {
            string baseTargetPath =
                eventNumber.ToString() + "_" +
                gameSubNumber.ToString() + "/" +
                targetPath;

            string baseDirectory =
                EnvironmentEntity.ContentRootPath +
                $@"/wwwroot/media/" +
                baseTargetPath +
                "now/";

            // LiveStreamingディレクトリの削除
            Directory.Delete(baseDirectory, true);

            while (Directory.Exists(baseDirectory));

            // 完了次第古いファイルが消えたことを通知
            await SendRecordingFinished(Clients);
        }

        private async void ChangeRecording(
            IHubContext<SignalRHub> hubcontext,
            string finishedFileDirectory,
            string nowFileDirectory,
            string webmFileDirectory,
            string numberPath,
            string m3u8FilePath
        )
        {
            try
            {
                string finishednumberFileDirectory =
                    finishedFileDirectory +
                    numberPath + "/";

                // webmのディレクトリとファイルをすべて削除
                Directory.Delete(webmFileDirectory, true);

                int roopCount = 1;
                while (Directory.Exists(finishednumberFileDirectory))
                {
                    finishednumberFileDirectory = finishednumberFileDirectory.Trim() + roopCount.ToString() + "/";
                    roopCount++;
                }

                // directoryを移動
                Directory.CreateDirectory(finishednumberFileDirectory);
                Directory.Delete(finishednumberFileDirectory);

                while (Directory.Exists(finishednumberFileDirectory)) { }

                CopyDirectoryAndContents.Copy(nowFileDirectory, finishednumberFileDirectory);

                // m3u8Fileの内容を取得してリスト化
                List<string> allLines = System.IO.File.ReadAllLines(m3u8FilePath).ToList<string>();

                // EventをVODにリライト
                allLines[3] = "#EXT-X-PLAYLIST-TYPE:VOD";
                allLines.Add("#EXT-X-ENDLIST");

                // フォルダ内Indexの作成
                // header部を文字列化
                List<string> medias = allLines.Take(5).ToList();

                // 該当フォルダの動画パスだけを抽出
                int firstIndex = allLines.FindIndex(x => x.Split("/")[0] == numberPath[..^1]) - 1;
                int lastIndex = allLines.FindLastIndex(x => x.Split("/")[0] == numberPath[..^1]) - 1;

                medias.AddRange(
                    allLines.Skip(firstIndex)
                    .Take(lastIndex)
                    .Select(x => x.Split("/").Length == 1 ? x : x.Split("/")[1])
                    .ToList()
                );

                medias.Add("#EXT-X-ENDLIST");

                string allM3u8FilePath = finishedFileDirectory + "index.m3u8";
                string newM3u8FilePath = finishednumberFileDirectory + "index.m3u8";

                // m3u8ファイルの上書き及び生成
                using (StreamWriter writer = new StreamWriter(allM3u8FilePath, false))
                {
                    writer.Write(String.Join("\r\n", allLines));
                }
                using (StreamWriter writer = new StreamWriter(newM3u8FilePath, false))
                {
                    writer.Write(String.Join("\r\n", medias));
                }

                await SendMoveFileFinished(hubcontext);
            }
            catch
            {
                throw;
            }
        }

        public async Task SendMoveFileFinished(IHubContext<SignalRHub> hubcontext)
        {
            await hubcontext.Clients.All.SendAsync("SendMoveFileFinished");
        }

        private static async Task SendRecordingFinished(IHubCallerClients Clients)
        {
            await Clients.All.SendAsync("SendRecordingFinished");
        }
    }
}