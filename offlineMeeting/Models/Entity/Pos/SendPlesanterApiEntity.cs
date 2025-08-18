using offlineMeeting.Models.JsonDataProperty;
using RazorEngine.Text;

namespace offlineMeeting.Models.Entity.Pos
{
    public class SendPlesanterApiEntity: ApiKeyProperty
    {
        public string ApiVersion { get; set; } = "1.1";
        public new string ApiKey { get; set; }
        public List<SendOrderEntity> Data { get; set; }

        public SendPlesanterApiEntity(List<SendOrderEntity> data)
        {
            ApiKey = base.ApiKey!;
            Data = data;
        }
    }
}
