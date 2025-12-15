namespace offlineMeeting.Models.Entity.Login
{
    public class GenerateQr
    {
        public long EventId { get; set; }
        public long UserId { get; set; }

        public GenerateQr(
            long eventId,
            long userId
        ) {
            EventId = eventId;
            UserId = userId;
        }
    }
}
