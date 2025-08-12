using System.Text.Json;
using offlineMeeting.Models.Share;
using offlineMeeting.Models.Entity.Share;

namespace offlineMeeting.Models.JsonDataProperty
{
    public class ApiKeyEntity
    {
        public string? ApiKey { get; set; }
    }

    public class ApiKeyProperty: ApiKeyEntity
    {
        public ApiKeyProperty()
        {
            var readJson = new ReadJson();
            var jsonData = readJson.Get(EnvironmentEntity.ContentRootPath + "/wwwroot/json/ApiKey.json", "utf-8");
            ApiKey = JsonSerializer.Deserialize<ApiKeyEntity>(jsonData)?.ApiKey ?? throw new NullReferenceException("json read error");
        }
    }
}
