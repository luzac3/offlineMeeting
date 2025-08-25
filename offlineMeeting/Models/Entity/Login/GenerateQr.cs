namespace offlineMeeting.Models.Entity.Login
{
    public class GenerateQr
    {
        public int EventId { get; set; }
        public int UserId { get; set; }

        public GenerateQr(
            int eventId,
            int userId
        ) {
            EventId = eventId;
            UserId = userId;
        }
    }
}
