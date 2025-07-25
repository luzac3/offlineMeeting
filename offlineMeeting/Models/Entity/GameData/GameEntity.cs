using offlineMeeting.Models.DBProperty;

namespace offlineMeeting.Models.Entity.GameData
{
    public class GameEntity
    {
        public int EventNumber { get; set; }
        public string? EventName { get; set; }
        public DateTime? EventDate { get; set; }
        public int GameNumber { get; set; }
        public int HandNumber { get; set; }
        public int HandSubNumber { get; set; }
        public bool InGame { get; set; }
        public List<UsersEntity>? Users { get; set; }

        public GameEntity() {
            GameNumber = 1;
            HandNumber = 1;
            HandSubNumber = 0;
            InGame = false;
        }

        public GameEntity(
            int eventNumber,
            string eventName,
            int gameNumber,
            int handNumber,
            int handSubNumber,
            List<UsersEntity> users,
            bool inGame = false
        )
        {
            EventNumber = eventNumber;
            EventName = eventName;
            GameNumber = gameNumber;
            HandNumber = handNumber;
            HandSubNumber = handSubNumber;
            Users = users;
            InGame = inGame;
        }
    }
}
