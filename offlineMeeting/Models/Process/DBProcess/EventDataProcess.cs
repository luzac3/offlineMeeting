using offlineMeeting.Models.DBEntity;
using offlineMeeting.Models.Share;
using Microsoft.EntityFrameworkCore;

namespace offlineMeeting.Models.Process.DBProcess
{
    public class EventDataProcess : DBContext
    {
        public DbSet<EventDataEntity> EventData { get; set; }

        public EventDataProcess() { }

        public List<EventDataEntity> Get()
        {
            return EventData.ToList();
        }

        public List<EventDataEntity> Get(int eventNumber)
        {
            var t = EventData
                .Where(e => e.EventNumber == eventNumber)
                .ToList() ??
                throw new Exception("invalid GetEventData");
            return EventData
                .Where(e => e.EventNumber == eventNumber)
                .ToList() ??
                throw new Exception("invalid GetEventData");
        }

        public bool Insert(int eventNumber, string eventName = "", DateTime? eventDate = null)
        {
            EventData.Add(
                new EventDataEntity(
                    eventNumber:  eventNumber,
                    eventName: eventName,
                    eventDate: eventDate
                )
            );
            SaveChanges();
            return true;
        }
    }
}
