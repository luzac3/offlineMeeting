using Microsoft.EntityFrameworkCore;

namespace offlineMeeting.Models.DBEntity
{
    [PrimaryKey(nameof(EventNumber))]
    public class EventDataEntity
    {
        public int EventNumber {  get; set; }
        public string? EventName { get; set;}
        public DateTime? EventDate { get; set; }
        public EventDataEntity() { }
        public EventDataEntity(
            int eventNumber,
            string eventName,
            DateTime? eventDate
        )
        {
            EventNumber = eventNumber;
            EventName = eventName;
            EventDate = eventDate;
        }
    }
}