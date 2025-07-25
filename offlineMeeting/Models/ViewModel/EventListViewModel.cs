using offlineMeeting.Models.DBProperty;

namespace offlineMeeting.Models.ViewModel
{
    public class EventListViewModel
    {
        public List<DBEntity.EventDataEntity>? EventDataEntityList { get; set; }
        public List<UsersEntity>? UsersEntityList { get; set; }

        public EventListViewModel() { }

        public void setEventDataEntity(List<DBEntity.EventDataEntity> eventDataEntityList)
        {
            EventDataEntityList = eventDataEntityList;
        }
        public void setUsersEntityList(List<UsersEntity> usersEntityList)
        {
            UsersEntityList = usersEntityList;
        }
    }
}