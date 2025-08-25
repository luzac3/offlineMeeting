using System.Text.Json;
using offlineMeeting.Models.Share;
using offlineMeeting.Models.Entity.Share;

namespace offlineMeeting.Models.JsonDataProperty
{
    public class TableIdEntity
    {
        public int SakeSiteId { get; set; }
        public int OrderSiteId { get; set; }
        public int JoinerSiteId { get; set; }
        public int CoinsSiteId { get; set; }
    }

    public class TableIdProperty : TableIdEntity
    {
        public TableIdProperty()
        {
            var readJson = new ReadJson();
            #if DEBUG
                var jsonData = readJson.Get(EnvironmentEntity.ContentRootPath + "/wwwroot/json/TableId_debug.json", "utf-8");
            #else
                var jsonData = readJson.Get(EnvironmentEntity.ContentRootPath + "/wwwroot/json/TableId_release.json", "utf-8");
            #endif

            SakeSiteId = JsonSerializer.Deserialize<TableIdEntity>(jsonData)?.SakeSiteId ?? throw new NullReferenceException("json read error");
            OrderSiteId = JsonSerializer.Deserialize<TableIdEntity>(jsonData)?.OrderSiteId ?? throw new NullReferenceException("json read error");
            JoinerSiteId = JsonSerializer.Deserialize<TableIdEntity>(jsonData)?.JoinerSiteId ?? throw new NullReferenceException("json read error");
            CoinsSiteId = JsonSerializer.Deserialize<TableIdEntity>(jsonData)?.CoinsSiteId ?? throw new NullReferenceException("json read error");
        }
    }
}