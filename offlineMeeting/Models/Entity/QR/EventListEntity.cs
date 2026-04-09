namespace offlineMeeting.Models.Entity.QR
{
    public class EventListEntity(long eventId, string eventName)
    {
        public long EventId { get; set; } = eventId;
        public string EventName { get; set; } = eventName;
    }
}
