using offlineMeeting.Models.JsonDataProperty;

namespace offlineMeeting.Models.ViewModel
{
    public class CoinsViewModel
    {
        public string ClientId { get; set; }
        public bool IsSandbox { get; set; }

        public CoinsViewModel()
        {
            PaypalProperty paypalProperty = new();
            ClientId = paypalProperty.ClientId ?? "";
            IsSandbox = paypalProperty.IsSandbox;
        }
    }
}