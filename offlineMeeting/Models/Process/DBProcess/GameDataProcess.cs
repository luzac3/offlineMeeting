using offlineMeeting.Models.DBEntity;
using offlineMeeting.Models.Share;
using Microsoft.EntityFrameworkCore;

namespace offlineMeeting.Models.Process.DBProcess
{
    public class GameDataProcess: DBContext
    {
        public DbSet<GameDataEntity> GameData { get; set; }

        public GameDataProcess() { }

        public List<GameDataEntity> Get()
        {
            return GameData.ToList();
        }

        public List<GameDataEntity> Get(int eventNumber)
        {
            return GameData
                .Where(e=>e.EventNumber == eventNumber)
                .ToList();
        }

        public GameDataEntity Get(int eventNumber, int gameNumber)
        {
            return GameData
                .Where(e => e.EventNumber == eventNumber && e.GameNumber == gameNumber)
                .First();
        }

        public void Insert(
            int eventNumber,
            int gameNumber,
            int handNumber,
            int handSubNumber,
            int leachCount = 0,
            DateTime? gameDate = null,
            bool inGame = false
        )
        {
            GameData.Add(
                new GameDataEntity(
                    eventNumber: eventNumber,
                    gameNumber: gameNumber,
                    handNumber: handNumber,
                    handSubNumber: handSubNumber,
                    leachCount: leachCount,
                    gameDate: gameDate,
                    inGame: inGame
                )
            );
            SaveChanges();
        }
        public void Update(
            int eventNumber,
            int gameNumber,
            bool inGame
        )
        {
            GameData
                .Where(
                    x => x.EventNumber == eventNumber &&
                    x.GameNumber == gameNumber
                 )
                .ExecuteUpdate(setters => setters
                    .SetProperty(y => y.InGame, inGame)
                );
            SaveChanges();
        }

        public void Update(
            int eventNumber,
            int gameNumber,
            int handNumber,
            int handSubNumber,
            int leachCount
        )
        {
            GameData
                .Where(
                    x => x.EventNumber == eventNumber &&
                    x.GameNumber == gameNumber
                 )
                .ExecuteUpdate(setters => setters
                    .SetProperty(y => y.HandNumber, handNumber)
                    .SetProperty(y => y.HandSubNumber, handSubNumber)
                    .SetProperty(y => y.LeachCount, leachCount));
            SaveChanges();
        }

        public void Update(
            int eventNumber,
            int gameNumber,
            int handNumber,
            int handSubNumber,
            int leachCount = 0,
            DateTime? gameDate = null,
            bool inGame = false
        )
        {
            GameData
                .Where(
                    x => x.EventNumber == eventNumber &&
                    x.GameNumber == gameNumber
                 )
                .ExecuteUpdate(setters => setters
                    .SetProperty(y => y.HandNumber, handNumber)
                    .SetProperty(y => y.HandSubNumber, handSubNumber)
                    .SetProperty(y => y.LeachCount, leachCount)
                    .SetProperty(y => y.GameDate, gameDate ?? DateTime.Now)
                    .SetProperty(y => y.InGame, inGame));
            SaveChanges();
        }
    }
}
