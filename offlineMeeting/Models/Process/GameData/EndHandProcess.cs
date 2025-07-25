using offlineMeeting.Models.DBEntity;
using offlineMeeting.Models.Entity.GameData;
using offlineMeeting.Models.Hubs;
using offlineMeeting.Models.Process.DBProcess;
using offlineMeeting.Models.Share;
using offlineMeeting.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.SignalR;
using System.Text.Json;

namespace offlineMeeting.Models.Process.GameData
{
    public class EndHandProcess
    {
        public GameDataViewModel EndHand(
            GameDataViewModel GameDataViewModel,
            EndHandRegisterPostEntity endHandRegisterPostEntity
        )
        {
            return Register(GameDataViewModel, endHandRegisterPostEntity);
        }

        public async void SetGameDataPartialViewAndEntity(
            ICompositeViewEngine ViewEngine,
            ActionContext Context,
            GameDataViewModel GameDataViewModel,
            PartialViewResult GameData,
            PartialViewResult Result,
            IHubContext<SignalRHub> hubContext
        )
        {
            ConvertPartialViewToJson convertPartialViewToJson = new ConvertPartialViewToJson();

            if (GameDataViewModel.EndHandResultEntityList == null)
            {
                throw new Exception("invalid EndHandResultEntityList");
            }

            string gameDataString = convertPartialViewToJson.convert(
                ViewEngine,
                Context,
                "_GameData",
                GameData
            );

            string resultString = convertPartialViewToJson.convert(
                ViewEngine,
                Context,
                "_Result",
                Result
            );

            await SendEndHandResultEntityList(GameDataViewModel.EndHandResultEntityList, hubContext);
            await SendEndHandJson(
                new Dictionary<string, string>
                {
                    { "gameData", gameDataString },
                    { "result", resultString }
                },
                hubContext
            );

        }

        private static async Task SendEndHandResultEntityList(List<EndHandResultEntity> endHandResultEntityList, IHubContext<SignalRHub> hubContext)
        {
            string endHandResultEntityListJson = JsonSerializer.Serialize(endHandResultEntityList);
            await hubContext.Clients.All.SendAsync("SendEndHandResultEntityList", endHandResultEntityList);
        }

        private static async Task SendEndHandJson(Dictionary<string, string> endHandDic, IHubContext<SignalRHub> hubContext)
        {
            string endHandJson = JsonSerializer.Serialize(endHandDic);
            await hubContext.Clients.All.SendAsync("SendEndHandJson", endHandDic);
        }

        private GameDataViewModel Register(
            GameDataViewModel GameDataViewModel,
            EndHandRegisterPostEntity endHandRegisterPostEntity
        )
        {
            InGameUsersProcess inGameUsersProcess = new InGameUsersProcess();
            GameUsersDataProcess gameUsersDataProcess = new GameUsersDataProcess();

            int eventNumber = endHandRegisterPostEntity.EventNumber;
            List<int> leachUserList = endHandRegisterPostEntity.LeachUsersList ?? new List<int> { };
            List<int> myaoUserList = endHandRegisterPostEntity.MyaoUsersList ?? new List<int> { };
            Dictionary<int,string> videoIdDic = endHandRegisterPostEntity.VideoIdDic ?? new Dictionary<int, string> { };
            Dictionary<int, string> videoNameDic = endHandRegisterPostEntity.VideoNameDic ?? new Dictionary<int, string> { };
            Dictionary<int, bool> validVideoDic = endHandRegisterPostEntity.ValidVideoDic ?? new Dictionary<int, bool> { };
            int parent = endHandRegisterPostEntity.Parent;
            int horaKind = endHandRegisterPostEntity.HoraKind;

            // ツモ関連の点数を登録
            SetTumoPoint(
                horaKind,
                endHandRegisterPostEntity.PickPointDic,
                out int tumoPoint,
                out int parentTumoPoint,
                out int pickUser
            );

            // gamedataから必要な情報を取得
            GameDataProcess gameDataProcess = new GameDataProcess();
            GameDataEntity gameDataEntity = gameDataProcess
                .Get(eventNumber)
                .OrderByDescending(x => x.GameNumber)?
                .FirstOrDefault() ??
                throw new Exception("invalid GameData");

            int gameNumber = gameDataEntity.GameNumber;
            int nowHandNumber = gameDataEntity.HandNumber;
            int nowHandSubNumber = gameDataEntity.HandSubNumber;
            int nextHandNumber = gameDataEntity.HandNumber;
            int nextHandSubNumber = gameDataEntity.HandSubNumber;

            // HandNumber等が複数回更新されることを防ぐため、フラグ管理 HandSubはFalseなら0になる
            bool isIncleaseHandNumber = true;
            bool isIncleaseHandSubNumber = false;

            // 記録されている分＋今回立直分 
            int nowLeachCount = gameDataEntity.LeachCount + leachUserList.Count;
            int nextLeachCount = nowLeachCount;

            // GameUsersDataからUserCdを取得
            List<GameUsersDataEntity> gameUsersDataEntityList
                = gameUsersDataProcess.Get(
                    eventNumber,
                    gameNumber
                );

            // 更新用のリストとEntityを作成
            List<GameUsersDataEntity> updateGameUsersDataEntityList = new List<GameUsersDataEntity> { };
            List<InGameUsersDataEntity> updateInGameUsersDataEntityList = new List<InGameUsersDataEntity> { };
            bool isNotParentChange = false;

            foreach (GameUsersDataEntity gameUsersDataEntity in gameUsersDataEntityList)
            {
                int userCd = gameUsersDataEntity.UserCd;
                int point = gameUsersDataEntity.Point;
                int directionCd = gameUsersDataEntity.Direction;

                bool isParent = endHandRegisterPostEntity.Parent == userCd;
                int getLeachCount = 0;
                int resultCd = 0;
                int movingPoint = 0;

                // 特殊流局
                if (horaKind > 30)
                {
                    // 特殊流局の場合、基本的にユーザーのデータはほぼ変化しない
                    nextHandSubNumber += 1;
                    resultCd = horaKind;
                }

                // 通常流局
                if (
                    endHandRegisterPostEntity.TenpaiUsersList != null &&
                    horaKind == 30
                )
                {
                    int tenpaiUsersCount = endHandRegisterPostEntity.TenpaiUsersList.Count;

                    switch (tenpaiUsersCount)
                    {
                        case 1:
                            movingPoint = endHandRegisterPostEntity.TenpaiUsersList.First() == userCd ?
                                movingPoint + 3000 :
                                movingPoint - 1000;
                            resultCd = endHandRegisterPostEntity.TenpaiUsersList.First() == userCd ?
                                31 :
                                32;
                            break;
                        case 2:
                            movingPoint = endHandRegisterPostEntity.TenpaiUsersList.Contains(userCd) ?
                                movingPoint + 1500 :
                                movingPoint - 1500;
                            resultCd = endHandRegisterPostEntity.TenpaiUsersList.Contains(userCd) ?
                                31 :
                                32;
                            break;
                        case 3:
                            movingPoint = endHandRegisterPostEntity.TenpaiUsersList.Contains(userCd) ?
                                movingPoint + 1000 :
                                movingPoint - 3000;
                            resultCd = endHandRegisterPostEntity.TenpaiUsersList.Contains(userCd) ?
                                31 :
                                32;
                            break;
                    }
                    if (endHandRegisterPostEntity.TenpaiUsersList.Contains(parent))
                    {
                        isNotParentChange = true;
                        isIncleaseHandNumber = false;
                    }
                    isIncleaseHandSubNumber = true;
                }

                // ツモ
                if (pickUser != 0)
                {
                    // 自分が和了者か
                    if (pickUser == userCd)
                    {
                        // 親ツモ
                        if (parentTumoPoint == 0)
                        {
                            movingPoint += tumoPoint * 3;

                            isIncleaseHandNumber = false;
                            isIncleaseHandSubNumber = true;
                            isNotParentChange = true;
                        }
                        else
                        {
                            movingPoint += tumoPoint * 2 + parentTumoPoint;
                            nextHandSubNumber = 0;
                        }
                        resultCd = 1;
                    }
                    else
                    {
                        // 和了者でなく、自分が親であれば常に親被り
                        movingPoint = isParent ?
                            movingPoint - parentTumoPoint :
                            movingPoint - tumoPoint;
                    }
                }

                // ロン
                if (
                    endHandRegisterPostEntity.RonPointDicList != null &&
                    horaKind == 2
                )
                {
                    foreach (Dictionary<string, int> ronPointDic in endHandRegisterPostEntity.RonPointDicList)
                    {
                        int horaUser = ronPointDic["UserCd"];
                        int dealInUser = ronPointDic["DealInUserCd"];

                        if (horaUser == userCd)
                        {
                            // fixme ここの処理について、いずれもダブロントリロンの場合に狂うので考える
                            // リー棒、積み棒について頭ハネを考えないといけない
                            movingPoint += ronPointDic["Point"];
                            if (parent == userCd)
                            {
                                isIncleaseHandNumber = false;
                                isIncleaseHandSubNumber = true;
                                isNotParentChange = true;
                            }
                            resultCd = 2;
                        }

                        if (dealInUser == userCd)
                        {
                            movingPoint -= ronPointDic["Point"];
                            resultCd = 3;
                        }
                    }
                }

                //pao

                // リーチしたユーザーは立直分点数が減る
                if (leachUserList.Contains(userCd))
                {
                    point -= 1000;
                }

                // リー棒があった場合は和了者に行く　ダブロンの処理は後で考える
                // 放銃者のDirectionを参照して近い順みたいなことになると思うが・・・
                if (
                    horaKind < 30 &&
                    movingPoint > 0
                )
                {
                    point += (nowLeachCount) * 1000;
                    nextLeachCount = 0;
                    getLeachCount = nowLeachCount;
                }

                // videoIdとNameを登録するために、Cdと連携が必要　よってDicで送る必要がある
                // Listに登録データを詰め込む
                updateGameUsersDataEntityList
                    .Add(
                        new GameUsersDataEntity(
                            eventNumber: eventNumber,
                            gameNumber: gameNumber,
                            userCd: userCd,
                            direction: directionCd,
                            point: point + movingPoint,
                            videoId: videoIdDic.ContainsKey(userCd) ? videoIdDic[userCd] : null,
                            videoNumber: gameUsersDataEntity.VideoNumber,
                            videoName: videoNameDic.ContainsKey(userCd) ? videoNameDic[userCd] : null,
                            validVideo: validVideoDic.ContainsKey(userCd) ? validVideoDic[userCd] : false
                        )
                    );

                updateInGameUsersDataEntityList
                    .Add(
                        new InGameUsersDataEntity(
                            eventNumber: eventNumber,
                            gameNumber: gameNumber,
                            handNumber: nowHandNumber,
                            handSubNumber: nowHandSubNumber,
                            userCd: userCd,
                            directionCd: directionCd,
                            isParent: isParent,
                            resultCd: resultCd,
                            point: movingPoint,
                            isLeach: leachUserList.Contains(userCd),
                            isMyao: myaoUserList.Contains(userCd),
                            getLeachCount: getLeachCount
                        )
                    );
            }

            foreach (GameUsersDataEntity gameUsersDataEntity in updateGameUsersDataEntityList)
            {
                // 親変更があればdirectionを更新する
                if (!isNotParentChange)
                {
                    int nextDirection = gameUsersDataEntity.Direction - 1;

                    gameUsersDataEntity.Direction = nextDirection <= 0 ? 4 : nextDirection;
                }
            }

            // HandNumberとHandSubNumberの増減を反映
            if (isIncleaseHandNumber)
            {
                nextHandNumber++;
            }
            if (isIncleaseHandSubNumber)
            {
                nextHandSubNumber++;
            }
            else
            {
                nextHandSubNumber = 0;
            }

            try
            {
                gameDataProcess.Update(
                    eventNumber: eventNumber,
                    gameNumber: gameNumber,
                    handNumber: nextHandNumber,
                    handSubNumber: nextHandSubNumber,
                    leachCount: nextLeachCount
                );

                gameUsersDataProcess.Update(
                    eventNumber: eventNumber,
                    gameNumber: gameNumber,
                    gameUsersDataEntityList: updateGameUsersDataEntityList,
                    isNotParentChange: isNotParentChange
                );

                inGameUsersProcess.Insert(updateInGameUsersDataEntityList);

            }
            catch
            {
                throw;
            }

            GameDataViewModel.setEndHandResultList(SetResultValue(
                    nextHandNumber: nextHandNumber,
                    nextHandSubNumber: nextHandSubNumber,
                    nowLeachCount: nowLeachCount,
                    nextLeachCount: nextLeachCount,
                    gameUsersDataEntityList: updateGameUsersDataEntityList,
                    inGameUsersDataEntityList: updateInGameUsersDataEntityList
                ));

            GameDataViewModel.setGameEntity(SetGameValue(
                    gameDataEntity: gameDataEntity,
                    nextHandNumber: nextHandNumber,
                    nextHandSubNumber: nextHandSubNumber,
                    gameUsersDataEntityList: updateGameUsersDataEntityList
                ));

            GameDataViewModel.setGameUsersDataEntityList(updateGameUsersDataEntityList);

            return GameDataViewModel;
        }

        private void SetTumoPoint(
            int horaKind,
            Dictionary<string, int>? PickPointDic,
            out int tumoPoint,
            out int parentTumoPoint,
            out int pickUser
        )
        {
            tumoPoint = 0;
            parentTumoPoint = 0;
            pickUser = 0;

            if (
                PickPointDic != null &&
                horaKind == 1
            )
            {
                if (PickPointDic.Count == 3)
                {
                    tumoPoint = PickPointDic["TumoPoint"];

                    parentTumoPoint = PickPointDic["ParentTumoPoint"];
                }
                else
                {
                    tumoPoint = PickPointDic["TumoPoint"];
                }
                pickUser = PickPointDic["UserCd"];
            }
        }

        private List<EndHandResultEntity> SetResultValue(
            int nextHandNumber,
            int nextHandSubNumber,
            int nowLeachCount,
            int nextLeachCount,
            List<GameUsersDataEntity> gameUsersDataEntityList,
            List<InGameUsersDataEntity> inGameUsersDataEntityList
        )
        {
            UsersProcess usersProcess = new UsersProcess();
            var userList = usersProcess.Get(
                gameUsersDataEntityList
                    .Select(x => x.UserCd)
                    .ToList()
            );

            var processList = gameUsersDataEntityList
                .Join(
                    inGameUsersDataEntityList,
                    x => x.UserCd,
                    y => y.UserCd,
                    (gude, igude) => new
                    {
                        gude.UserCd,
                        gude.Direction,
                        NowHandNumber = igude.HandNumber,
                        NowHandSubNumber = igude.HandSubNumber,
                        NextHandNumber = nextHandNumber,
                        NextHandSubNumber = nextHandSubNumber,
                        igude.ResultCd,
                        MovingPoint = igude.Point,
                        gude.Point,
                        NowLeachCount = nowLeachCount,
                        NextLeachCount = nextLeachCount,
                        igude.IsLeach,
                        igude.IsMyao,
                        gude.VideoId,
                        gude.VideoNumber,
                        gude.VideoName
                    }
                ).Join(
                    userList,
                    x => x.UserCd,
                    y => y.UserCd,
                    (ul, nl) => new EndHandResultEntity(
                       ul.UserCd,
                        ul.Direction,
                        ul.NowHandNumber,
                        ul.NowHandSubNumber,
                        ul.NextHandNumber,
                        ul.NextHandSubNumber,
                        ul.ResultCd,
                        ul.MovingPoint,
                        ul.Point,
                        ul.NowLeachCount,
                        ul.NextLeachCount,
                        ul.IsLeach,
                        ul.IsMyao,
                        nl.UserName ?? "",
                        nl.UserTitle ?? "",
                        ul.VideoNumber
                    )
                ).ToList();

            return processList;
        }

        private GameEntity SetGameValue(
            GameDataEntity gameDataEntity,
            int nextHandNumber,
            int nextHandSubNumber,
            List<GameUsersDataEntity> gameUsersDataEntityList
        )
        {
            EventDataProcess eventDataProcess = new EventDataProcess();

            // eventNameの取得
            string eventName = eventDataProcess
                .Get(gameDataEntity.EventNumber)?
                .FirstOrDefault()?
                .EventName?
                .ToString() ??
                "未定";

            UsersProcess usersProcess = new UsersProcess();
            var users = usersProcess.Get(
                gameUsersDataEntityList
                    .Select(x => x.UserCd)
                    .ToList()
            );

            if (users.Count == 0)
            {
                throw new Exception("invalid userList");
            }

            GameEntity gameEntity = new GameEntity(
                eventNumber: gameDataEntity.EventNumber,
                eventName: eventName,
                gameNumber: gameDataEntity.GameNumber,
                handNumber: nextHandNumber,
                handSubNumber: nextHandSubNumber,
                users: users,
                inGame: true
            );

            return gameEntity;
        }
    }
}