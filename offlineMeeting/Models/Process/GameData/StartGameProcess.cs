using offlineMeeting.Models.DBEntity;
using offlineMeeting.Models.Entity.GameData;
using offlineMeeting.Models.Process.DBProcess;
using offlineMeeting.Models.Share;
using offlineMeeting.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;

namespace offlineMeeting.Models.Process.GameData
{
    public class StartGameProcess
    {
        public GameDataViewModel startGame(
            GameDataViewModel GameDataViewModel,
            StartGamePostEntity startGamePostEntity
        )
        {
            if(startGamePostEntity.HandNumber > 1 || startGamePostEntity.HandSubNumber > 0)
            {
                return setStartHandData(GameDataViewModel, startGamePostEntity);
            }
            else
            {
                return setStartGameData(GameDataViewModel, startGamePostEntity);
            }
        }

        public Dictionary<string, string> SetGameDataPartialView(
            ICompositeViewEngine ViewEngine,
            ActionContext Context,
            PartialViewResult GameData,
            PartialViewResult VideoController
        )
        {
            ConvertPartialViewToJson convertPartialViewToJson = new ConvertPartialViewToJson();

            string gameDataString = convertPartialViewToJson.convert(
                ViewEngine,
                Context,
                "_GameData",
                GameData
            );

            string videoControllString = convertPartialViewToJson.convert(
                ViewEngine,
                Context,
                "_VideoController",
                VideoController
            );

            return new Dictionary<string, string> {
                {"gameData", gameDataString},
                {"videoController", videoControllString}
            };
        }

        private GameDataViewModel setStartGameData(GameDataViewModel GameDataViewModel, StartGamePostEntity startGamePostEntity)
        {
            int eventNumber = startGamePostEntity.EventNumber;

            // イベント名などは変更されることがなく、再取得は不要
            GameDataProcess gameDataProcess = new GameDataProcess();
            GameUsersDataProcess gameUsersDataProcess = new GameUsersDataProcess();

            GameDataEntity gameDataEntity = gameDataProcess
                .Get(eventNumber)
                .OrderByDescending(x => x.GameNumber)
                .FirstOrDefault() ??
                throw new Exception("get gameData Error");

            if (startGamePostEntity.UsersList == null)
            {
                throw new Exception("userList is empty");
            }

            if (startGamePostEntity.DirectionDictionary == null)
            {
                throw new Exception("DirectionDictionary is empty");
            }

            try
            {
                // GameDataのInGameをTrueに更新
                gameDataProcess.Update(
                    eventNumber: eventNumber,
                    gameNumber: gameDataEntity.GameNumber,
                    handNumber: 1,
                    handSubNumber: 0,
                    gameDate: DateTime.Now,
                    inGame: true
                );

                // GameUsersDataを初期登録
                GameUsersDataEntity gameUsersDataEntity = new GameUsersDataEntity();

                List<GameUsersDataEntity> gameUsersDataEntityList = startGamePostEntity.UsersList.Select(x =>
                    new GameUsersDataEntity(
                        eventNumber: eventNumber,
                        gameNumber: gameDataEntity.GameNumber,
                        userCd: x,
                        direction: startGamePostEntity.DirectionDictionary[x],
                        point: 25000,
                        videoId: startGamePostEntity.VideoDictionary?[x],
                        videoNumber: startGamePostEntity.VideoNumberDictionary?[x]
                    )
                ).ToList();

                gameUsersDataProcess.Insert(gameUsersDataEntityList);
            }
            catch 
            {
                throw;
            }

            // 画面表示用のデータを取得して作成
            SetGameDataProcess setGameDataProcess = new SetGameDataProcess();

            return setGameDataProcess.Set(GameDataViewModel, eventNumber);
        }

        private GameDataViewModel setStartHandData(GameDataViewModel GameDataViewModel, StartGamePostEntity startGamePostEntity)
        {
            // 画面表示用のデータを取得して作成
            SetGameDataProcess setGameDataProcess = new SetGameDataProcess();

            return setGameDataProcess.Set(GameDataViewModel, startGamePostEntity.EventNumber);
        }
    }
}
