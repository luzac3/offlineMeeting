using System.Text.Json;
using offlineMeeting.Models.Share;
using offlineMeeting.Models.Entity.Share;

namespace offlineMeeting.Models.JsonDataProperty
{
    public class VoiceVoxSettingsEntity
    {
        public string BaseUrl { get; set; } = "http://127.0.0.1:50021";
        public int Speaker { get; set; }
        public double SpeedScale { get; set; } = 1.0;
        public double PitchScale { get; set; } = 0.0;
        public double IntonationScale { get; set; } = 1.0;
        public double VolumeScale { get; set; } = 1.0;
        public double PrePhonemeLength { get; set; } = 0.1;
        public double PostPhonemeLength { get; set; } = 0.1;
        public int TimeoutSeconds { get; set; } = 10;
    }

    public class VoiceVoxSettingsProperty : VoiceVoxSettingsEntity
    {
        public VoiceVoxSettingsProperty()
        {
            var readJson = new ReadJson();
            var jsonData = readJson.Get(EnvironmentEntity.ContentRootPath + "/wwwroot/json/VoiceVoxSettings.json", "utf-8");
            var entity = JsonSerializer.Deserialize<VoiceVoxSettingsEntity>(jsonData) ?? throw new NullReferenceException("json read error");

            BaseUrl = entity.BaseUrl;
            Speaker = entity.Speaker;
            SpeedScale = entity.SpeedScale;
            PitchScale = entity.PitchScale;
            IntonationScale = entity.IntonationScale;
            VolumeScale = entity.VolumeScale;
            PrePhonemeLength = entity.PrePhonemeLength;
            PostPhonemeLength = entity.PostPhonemeLength;
            TimeoutSeconds = entity.TimeoutSeconds;
        }
    }
}
