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
        private static readonly string JsonPath = "/wwwroot/json/Paypal.json";
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
                var filePath = EnvironmentEntity.ContentRootPath + JsonPath;
                var entity = ReadEntity();
                entity.IsSandbox = !entity.IsSandbox;
                var options = new JsonSerializerOptions { WriteIndented = true };
                File.WriteAllText(filePath, JsonSerializer.Serialize(entity, options));
            }
        }

        private static PaypalEntity ReadEntity()
        {
            var readJson = new ReadJson();
            var jsonData = readJson.Get(EnvironmentEntity.ContentRootPath + JsonPath, "utf-8");
            return JsonSerializer.Deserialize<PaypalEntity>(jsonData) ?? throw new NullReferenceException("json read error");
        }
    }
}
