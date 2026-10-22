namespace offlineMeeting.Models.Entity.QR
{
    public class CheckinRequest
    {
        public long Id { get; set; }
        public long UserCd { get; set; }
        public string UserName { get; set; } = "";
        public string PhoneticName { get; set; } = "";
        public string QrUrl { get; set; } = "";
    }
}