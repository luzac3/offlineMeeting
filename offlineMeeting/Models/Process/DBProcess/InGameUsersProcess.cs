using offlineMeeting.Models.DBEntity;
using offlineMeeting.Models.DBProperty;
using offlineMeeting.Models.Share;
using Microsoft.EntityFrameworkCore;

namespace offlineMeeting.Models.Process.DBProcess
{
    public class InGameUsersProcess : DBContext
    {
        public DbSet<UsersEntity> Users { get; set; }
        
        public DbSet<InGameUsersDataEntity> InGameUsersData { get; set; }

        public InGameUsersProcess() { }

        public List<InGameUsersDataEntity> Get()
        {
            return InGameUsersData.ToList();
        }

        public List<InGameUsersDataEntity> Get(
            int eventNumber,
            int gameNumber,
            int userCd
        )
        {
            return InGameUsersData
                .Where(x =>
                    x.EventNumber == eventNumber &&
                    x.GameNumber == gameNumber &&
                    x.UserCd == userCd
                )
                .OrderBy(x => x.EventNumber)
                .ThenBy(x => x.GameNumber)
                .ThenBy(x => x.HandNumber)
                .ThenBy(x => x.HandSubNumber)
                .ToList();
        }

        public List<InGameUsersDataEntity> Get(
            int eventNumber,
            int gameNumber,
            int handNumber,
            int handSubNumber
        )
        {
            return InGameUsersData
                .Where(x =>
                    x.EventNumber == eventNumber &&
                    x.GameNumber == gameNumber &&
                    x.HandNumber == handNumber &&
                    x.HandSubNumber == handSubNumber
                )
                .ToList();
        }

        public List<InGameUsersDataEntity> Get(
            int eventNumber,
            int gameNumber,
            int handNumber,
            int handSubNumber,
            int userCd
        )
        {
            return InGameUsersData
                .Where(x=>
                    x.EventNumber == eventNumber &&
                    x.GameNumber == gameNumber &&
                    x.HandNumber == handNumber &&
                    x.HandSubNumber == handSubNumber &&
                    x.UserCd == userCd
                )
                .ToList();
        }

        public bool Insert(
            int eventNumber,
            int gameNumber,
            int handNumber,
            int handSubNumber,
            int userCd, 
            int directionCd,
            bool isParent,
            int resultCd = 0,
            int point = 0,
            bool isLeach = false,
            bool isMyao = false,
            int getLeachCount = 0
        )
        {
            InGameUsersData.Add(
                new InGameUsersDataEntity(
                    eventNumber: eventNumber,
                    gameNumber: gameNumber,
                    handNumber: handNumber,
                    handSubNumber: handSubNumber,
                    userCd: userCd,
                    directionCd: directionCd,
                    isParent: isParent,
                    resultCd: resultCd,
                    point: point,
                    isLeach: isLeach,
                    isMyao: isMyao,
                    getLeachCount: getLeachCount
                )
            );
            SaveChanges();
            return true;
        }
        public bool Insert(List<InGameUsersDataEntity> inGameUserDataEntityList)
        {
            InGameUsersData.AddRange(inGameUserDataEntityList);
            SaveChanges();
            return true;
        }
    }
}