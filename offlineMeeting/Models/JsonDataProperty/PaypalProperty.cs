using System.Text.Json;
using offlineMeeting.Models.Share;
using offlineMeeting.Models.Entity.Share;

namespace offlineMeeting.Models.JsonDataProperty
{
    public class PaypalEntity
    {
        public string? SandboxClientId { get; set; }
        public string? LiveClientId { get; set; }
        public bool IsSandbox { get; set; }
    }

    public class PaypalProperty
    {
        private static readonly string OriginalJsonPath = "/wwwroot/json/Paypal.json";
        private static readonly string WritableJsonPath = "/data/Paypal.json";
        private static readonly object _lock = new();

        public string? ClientId { get; set; }
        public bool IsSandbox { get; set; }

        public PaypalProperty()
        {
            var entity = ReadEntity();
            IsSandbox = entity.IsSandbox;
            ClientId = entity.IsSandbox
                ? entity.SandboxClientId
                : entity.LiveClientId;
        }

        public static bool GetIsSandbox()
        {
            return ReadEntity().IsSandbox;
        }

        public static void ToggleMode()
        {
            lock (_lock)
            {
                var filePath = GetWritableFilePath();
                var entity = ReadEntity();
                entity.IsSandbox = !entity.IsSandbox;
                var options = new JsonSerializerOptions { WriteIndented = true };
                File.WriteAllText(filePath, JsonSerializer.Serialize(entity, options));
            }
        }

        private static string GetWritableFilePath()
        {
            var writablePath = EnvironmentEntity.ContentRootPath + WritableJsonPath;
            if (!File.Exists(writablePath))
            {
                var dir = Path.GetDirectoryName(writablePath);
                if (!string.IsNullOrEmpty(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                var originalPath = EnvironmentEntity.ContentRootPath + OriginalJsonPath;
                File.Copy(originalPath, writablePath);
            }
            return writablePath;
        }

        private static PaypalEntity ReadEntity()
        {
            var writablePath = EnvironmentEntity.ContentRootPath + WritableJsonPath;
            var readJson = new ReadJson();

            // 書き込み可能パスにファイルがあればそちらを優先
            var jsonPath = File.Exists(writablePath)
                ? writablePath
                : EnvironmentEntity.ContentRootPath + OriginalJsonPath;

            var jsonData = readJson.Get(jsonPath, "utf-8");
            return JsonSerializer.Deserialize<PaypalEntity>(jsonData) ?? throw new NullReferenceException("json read error");
        }
    }
}
