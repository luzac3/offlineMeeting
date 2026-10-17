namespace offlineMeeting.Models.Entity.QR
{
    public class CheckinJoinerEntity(long id, string userCd, string name, string url)
    {
        public long Id { get; set; } = id;
        public string UserCd { get; set; } = userCd;
        public string Name { get; set; } = name;
        public string Url { get; set; } = url;
    }
}
