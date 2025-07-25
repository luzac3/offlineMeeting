using offlineMeeting.Models.DBEntity;
using offlineMeeting.Models.DBProperty;
using offlineMeeting.Models.Entity.GameData;
using offlineMeeting.Models.Process.DBProcess;

namespace offlineMeeting.Models.Process.GameData
{
    public class GetVideoUsersProcess
    {
        public object GetUsers(StartVideoEntity startVideoEntity)
        {
            GameUsersDataProcess gameUsersDataProcess = new GameUsersDataProcess();
            UsersProcess usersProcess = new UsersProcess();

            List<GameUsersDataEntity> gameUsersDataEntityList = gameUsersDataProcess.Get(startVideoEntity.EventNumber, startVideoEntity.GameNumber);
            List<int> userCdList = gameUsersDataEntityList.Select(x => x.UserCd).ToList();
            List<UsersEntity> usersEntityList = usersProcess.Get(userCdList);

            object returnUsersEntityList =gameUsersDataEntityList
                .OrderBy(x => x.VideoNumber)
                .Join(
                    usersEntityList,
                    x => x.UserCd,
                    y => y.UserCd,
                    (gudel, uel) => new
                    {
                        uel.UserCd,
                        uel.UserName,
                        uel.UserTitle,
                        gudel.Point
                    }
                )
                .ToList();

            return returnUsersEntityList;
        }
    }
}