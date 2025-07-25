using System.Text.Json;
using offlineMeeting.Models.Share;

namespace offlineMeeting.Models.JsonDataProperty
{
    public class DBConnectEntity
    {
        public string? Server { get; set; }
        public string? Database { get; set; }
        public string? UserId { get; set; }
        public string? Password { get; set; }
        public string? SslMode { get; set; }
    }

    public class DBConnectProperty: DBConnectEntity
    {
        public DBConnectProperty(string contentRootPath)
        {
            var readJson = new ReadJson();
            var jsonData = readJson.Get(contentRootPath + "/wwwroot/json/DBConnect.json", "utf-8");
            var dbConnectJson = JsonSerializer.Deserialize<DBConnectEntity>(jsonData);
            if ( dbConnectJson != null )
            {
                Server = dbConnectJson.Server;
                Database = dbConnectJson.Database;
                UserId = dbConnectJson.UserId;
                Password = dbConnectJson.Password;
                SslMode = dbConnectJson.SslMode;
            }
        }
    }
}
