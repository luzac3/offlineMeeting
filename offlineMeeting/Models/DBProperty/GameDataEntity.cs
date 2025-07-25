using Microsoft.EntityFrameworkCore;

namespace offlineMeeting.Models.DBEntity
{
    [PrimaryKey(nameof(EventNumber), nameof(GameNumber))]
    public class GameDataEntity
    {
        public int EventNumber { get; set; }
        public int GameNumber { get; set; }
        public int HandNumber { get; set; }
        public int HandSubNumber { get; set; }
        public int LeachCount { get; set; }
        public DateTime? GameDate { get; set; }
        public bool InGame { get; set; }
        public GameDataEntity() { }
        public GameDataEntity(
            int eventNumber,
            int gameNumber,
            int handNumber,
            int handSubNumber,
            int leachCount,
            DateTime? gameDate,
            bool inGame
        )
        {
            EventNumber = eventNumber;
            GameNumber = gameNumber;
            HandNumber = handNumber;
            HandSubNumber = handSubNumber;
            LeachCount = leachCount;
            GameDate = gameDate;
            InGame = inGame;
        }
    }
}