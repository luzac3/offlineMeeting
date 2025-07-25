using Microsoft.EntityFrameworkCore;

namespace offlineMeeting.Models.DBProperty
{
    [PrimaryKey(nameof(EventNumber), nameof(UserCd))]
    public class EventUsersEntity
    {
        public int EventNumber {  get; set; }
        public int UserCd { get; set;}
        public EventUsersEntity() { }
        public EventUsersEntity(
            int eventNumber,
            int userCd
        )
        {
            EventNumber = eventNumber;
            UserCd = userCd;
        }
    }
}