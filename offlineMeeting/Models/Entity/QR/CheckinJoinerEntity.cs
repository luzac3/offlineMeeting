namespace offlineMeeting.Models.Entity.QR
{
    public class CheckinJoinerEntity(long id, string userCd, string name, string phoneticName, string url, bool checkedIn)
    {
        public long Id { get; set; } = id;
        public string UserCd { get; set; } = userCd;
        public string Name { get; set; } = name;
        public string PhoneticName { get; set; } = phoneticName;
        public string Url { get; set; } = url;
        public bool CheckedIn { get; set; } = checkedIn;
    }
}
