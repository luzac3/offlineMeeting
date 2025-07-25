using System.Text.Json;
using offlineMeeting.Models.Share;
using offlineMeeting.Models.Entity.Share;

namespace offlineMeeting.Models.JsonDataProperty
{
    public class DirectionEntity
    {
        public Dictionary<int,string>? Directions { get; set; }
    }

    public class DirectionProperty: DirectionEntity
    {
        public DirectionProperty()
        {
            var readJson = new ReadJson();
            var jsonData = readJson.Get(EnvironmentEntity.ContentRootPath + "/wwwroot/json/Direction.json", "utf-8");
            Directions = JsonSerializer.Deserialize<DirectionEntity>(jsonData)?.Directions ?? throw new NullReferenceException("json read error");
        }
    }
}
