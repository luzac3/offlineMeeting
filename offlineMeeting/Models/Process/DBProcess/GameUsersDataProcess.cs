using offlineMeeting.Models.DBEntity;
using offlineMeeting.Models.Share;
using Microsoft.EntityFrameworkCore;

namespace offlineMeeting.Models.Process.DBProcess
{
    public class GameUsersDataProcess : DBContext
    {
        public DbSet<GameUsersDataEntity> GameUsersData { get; set; }

        public GameUsersDataProcess() { }

        public List<GameUsersDataEntity> Get()
        {
            return GameUsersData.ToList();
        }

        public List<GameUsersDataEntity> Get(
            int eventNumber,
            int gameNumber
        )
        {
            return GameUsersData
                .Where(x =>
                    x.EventNumber == eventNumber &&
                    x.GameNumber == gameNumber
                )
                .ToList();
        }

        public GameUsersDataEntity Get(
            int eventNumber,
            int gameNumber,
            int? userCd,
            int? videoNumber
        )
        {
            if(userCd is null)
            {
                return GameUsersData
                    .Where(x =>
                        x.EventNumber == eventNumber &&
                        x.GameNumber == gameNumber &&
                        x.VideoNumber == videoNumber
                    )?
                    .FirstOrDefault() ?? throw new Exception("invalid userCd");
            }
            else
            {
                return GameUsersData
                    .Where(x =>
                        x.EventNumber == eventNumber &&
                        x.GameNumber == gameNumber &&
                        x.UserCd == userCd
                    )?
                    .FirstOrDefault() ?? throw new Exception("invalid userCd");
            }
        }

        public bool Insert(
            int eventNumber,
            int gameNumber,
            int userCd,
            int direction,
            int point = 0,
            string? videoId = null
        )
        {
            GameUsersData.Add(
                new GameUsersDataEntity(
                    eventNumber: eventNumber,
                    gameNumber: gameNumber,
                    userCd: userCd,
                    direction: direction,
                    point: point,
                    videoId: videoId
                )
            );
            SaveChanges();
            return true;
        }

        public bool Insert(
            List<GameUsersDataEntity> gameUsersDataEntity
        )
        {
            GameUsersData.AddRange(gameUsersDataEntity);
            SaveChanges();
            return true;
        }

        public void Update(
            int eventNumber,
            int gameNumber,
            int userCd,
            int point = 0
        )
        {
            GameUsersData
                .Where(
                    x => x.EventNumber == eventNumber &&
                    x.GameNumber == gameNumber &&
                    x.UserCd == userCd
                 )
                .ExecuteUpdate(setters => setters
                    .SetProperty(y => y.Point, point)
                    );
            SaveChanges();
        }
        public void Update(
            int eventNumber,
            int gameNumber,
            List<GameUsersDataEntity> gameUsersDataEntityList,
            bool isNotParentChange
        )
        {
            foreach (var gameUsersDataEntity in gameUsersDataEntityList)
            {
                GameUsersDataEntity updateGameUsersDataEntity = GameUsersData
                    .Where(x =>
                        x.EventNumber == eventNumber &&
                        x.GameNumber == gameNumber
                    )
                    .Single(x => x.UserCd == gameUsersDataEntity.UserCd);

                updateGameUsersDataEntity.Point = gameUsersDataEntity.Point;
                updateGameUsersDataEntity.Direction = gameUsersDataEntity.Direction;
                updateGameUsersDataEntity.VideoId = gameUsersDataEntity.VideoId;
                updateGameUsersDataEntity.VideoNumber = gameUsersDataEntity.VideoNumber;
                updateGameUsersDataEntity.VideoName = gameUsersDataEntity.VideoName;
            }
            SaveChanges();
        }
    }
}