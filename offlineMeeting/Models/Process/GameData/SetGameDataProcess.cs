using offlineMeeting.Models.DBEntity;
using offlineMeeting.Models.DBProperty;
using offlineMeeting.Models.Entity.GameData;
using offlineMeeting.Models.Process.DBProcess;
using offlineMeeting.Models.ViewModel;


namespace offlineMeeting.Models.Process.GameData
{

    public class SetGameDataProcess
    {
        public SetGameDataProcess() { }

        public GameDataViewModel Set(GameDataViewModel GameDataViewModel, int eventNumber)
        {
            GameDataProcess gameDataProcess = new GameDataProcess();
            EventDataProcess eventDataProcess = new EventDataProcess();
            EventUsersProcess eventUsersProcess = new EventUsersProcess();
            GameUsersDataProcess gameUsersDataProcess = new GameUsersDataProcess();

            GameDataEntity gameDataEntity = gameDataProcess
                .Get(eventNumber)
                .OrderByDescending(x => x.GameNumber)
                .FirstOrDefault() ??
                throw new Exception("get gameData Error");

            // eventNameも取得
            string eventName = eventDataProcess
                .Get(eventNumber)?
                .FirstOrDefault()?
                .EventName?
                .ToString() ??
                "未定";

            List<GameUsersDataEntity> gameUsersDataEntityList = new List<GameUsersDataEntity> ();

            // InGameの場合、Userのゲームデータを取得する
            if (gameDataEntity.InGame)
            {
                gameUsersDataEntityList = gameUsersDataProcess
                    .Get(eventNumber, gameDataEntity.GameNumber);
            }

            // GameUsersの情報をセット
            GameDataViewModel.setGameUsersDataEntityList(gameUsersDataEntityList);

            // userCd:UserNameのDictionaryを取得
            List<UsersEntity>? users = eventUsersProcess.GetEventUsers(eventNumber);

            // GameEntityをセット
            GameDataViewModel.setGameEntity(new GameEntity(
                eventNumber: eventNumber,
                eventName: eventName,
                gameNumber: gameDataEntity.GameNumber,
                handNumber: gameDataEntity.HandNumber,
                handSubNumber: gameDataEntity.HandSubNumber,
                users: users,
                inGame: gameDataEntity.InGame
            ));

            return GameDataViewModel;
        }
    }
}