using offlineMeeting.Models.Entity.QR;

namespace offlineMeeting.Models.ViewModel
{
    public class PrintQRViewModel
    {
        public string UserName { get; set; }
        public string QrUrl { get; set; }

    public PrintQRViewModel(string userName, string qrUrl)
        {
            UserName = userName;
            QrUrl = qrUrl;
        }
    }
}