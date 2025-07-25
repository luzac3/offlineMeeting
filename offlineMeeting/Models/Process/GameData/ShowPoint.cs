using offlineMeeting.Models.DBEntity;
using offlineMeeting.Models.DBProperty;
using offlineMeeting.Models.Entity.GameData;
using offlineMeeting.Models.Process.DBProcess;

namespace offlineMeeting.Models.Process.GameData
{
    public class ShowPoint
    {
        public List<ShowPointResultEntity> Show(ShowPointPostEntity showPointPostEntity)
        {
            // 基本的にはユーザ情報とゲーム情報取得、ResultにつめてResultのViewを流すでOK
            GameDataProcess gameDataProcess = new GameDataProcess();
            GameUsersDataProcess gameUsersDataProcess = new GameUsersDataProcess();
            UsersProcess usersProcess = new UsersProcess();

            int eventNumber = showPointPostEntity.EventNumber;
            int gameNumber = showPointPostEntity.GameNumber;

            GameDataEntity gameDataEntity = gameDataProcess.Get(eventNumber: eventNumber, gameNumber: gameNumber);
            List<GameUsersDataEntity> GameUsersDataEntityList = gameUsersDataProcess.Get(eventNumber: eventNumber, gameNumber: gameNumber);
            List<UsersEntity> usersEntityList = usersProcess.Get(
                GameUsersDataEntityList.Select(x=>x.UserCd).ToList()
            );

            List<ShowPointResultEntity> showPointResultEntityList = new List<ShowPointResultEntity> { };

            foreach (GameUsersDataEntity gameUsersDataEntity in GameUsersDataEntityList)
            {
                UsersEntity usersEntity = usersEntityList.Single(x => x.UserCd == gameUsersDataEntity.UserCd);
                showPointResultEntityList.Add(
                    new ShowPointResultEntity(
                        handNumber: gameDataEntity.HandNumber,
                        handSubNumber: gameDataEntity.HandSubNumber,
                        userCd: gameUsersDataEntity.UserCd,
                        direction: gameUsersDataEntity.Direction,
                        point: gameUsersDataEntity.Point,
                        nowLeachCount: gameDataEntity.LeachCount,
                        userName: usersEntity.UserName ?? "",
                        userTitle: usersEntity.UserTitle ?? "",
                        videoNumber: gameUsersDataEntity.VideoNumber
                    )
                );
            }return showPointResultEntityList;
        }
    }
}