using offlineMeeting.Models.JsonDataProperty;
using RazorEngine.Text;

namespace offlineMeeting.Models.Entity.Pos
{
    public class SendPlesanterApiEntity: ApiKeyProperty
    {
        public string ApiVersion { get; set; } = "1.1";
        public new string ApiKey { get; set; }
        public List<SendRegisterEntity> Data { get; set; }

        public SendPlesanterApiEntity(List<SendRegisterEntity> data)
        {
            ApiKey = base.ApiKey!;
            Data = data;
        }
    }
}
