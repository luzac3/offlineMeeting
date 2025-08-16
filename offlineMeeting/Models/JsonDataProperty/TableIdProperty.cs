using System.Text.Json;
using offlineMeeting.Models.Share;
using offlineMeeting.Models.Entity.Share;

namespace offlineMeeting.Models.JsonDataProperty
{
    public class TableIdEntity
    {
        public int SakeSiteId { get; set; }
    }

    public class TableIdProperty : TableIdEntity
    {
        public TableIdProperty()
        {
            var readJson = new ReadJson();
            var jsonData = readJson.Get(EnvironmentEntity.ContentRootPath + "/wwwroot/json/ApiKey.json", "utf-8");
            SakeSiteId = JsonSerializer.Deserialize<TableIdEntity>(jsonData)?.SakeSiteId ?? throw new NullReferenceException("json read error");
        }
    }
}
