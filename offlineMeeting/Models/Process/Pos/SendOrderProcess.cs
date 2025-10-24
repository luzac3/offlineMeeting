using Microsoft.AspNetCore.SignalR;
using offlineMeeting.Models.DBProperty;
using offlineMeeting.Models.Entity.Login;
using offlineMeeting.Models.Entity.Pos;
using offlineMeeting.Models.Entity.Pos.SendOrderEntity;
using offlineMeeting.Models.Entity.Pos.SendOrderPurchaseEntity;
using offlineMeeting.Models.Entity.Share;
using offlineMeeting.Models.Process.DBProcess;
using offlineMeeting.Models.Process.Share;
using offlineMeeting.Models.Share;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Text.Json;

namespace offlineMeeting.Models.Process.Pos
{
    public class SendOrderProcess : ResultsProcess
    {
        private int OrderSiteId;
        private int JoinerSiteId;
        private string EventId;
        private HttpContext HttpContext;
        private List<OrderPostEntity> OrderPostEntityList;
        private readonly IHubContext<MyHub> _hubContext;

        public SendOrderProcess(List<OrderPostEntity> orderPostEntityList, int orderSiteId, int joinerSiteId, HttpContext httpContext, IHubContext<MyHub> hubContext)
        {
            ManageEventId manageEventId = new ManageEventId(httpContext);

            EventId = manageEventId.GetEventId();
            if (EventId == "0")
            {
                throw new DataException("イベントIDが不正です");
            }
            OrderSiteId = orderSiteId;
            JoinerSiteId = joinerSiteId;
            HttpContext = httpContext;
            OrderPostEntityList = orderPostEntityList.OrderBy(x => x.ResultId).ToList();
            _hubContext = hubContext;
        }


        public async Task<string> Send(int sakeSiteId)
        {
            List<SendOrderPurchaseEntity> sendOrderPurchaseEntityList = new List<SendOrderPurchaseEntity>();
            List<OrderPurchaseTableEntity> orderPurchaseTableEntity = new List<OrderPurchaseTableEntity>();
            ManageUserCd manageUserCd = new ManageUserCd(HttpContext);
            string userCd = manageUserCd.GetUserCd().ToString();
            if (userCd == "0")
            {
                return JsonSerializer.Serialize(new Dictionary<string, string> { { "status", "400" }, { "message", "ユーザコードがありません" } });
            }

            // コインが足りることを確認する
            int totalCoins = 0;

            GetUsersProcess getUsersProcess = new(JoinerSiteId, HttpContext);
            JoinerEntity userData = getUsersProcess.GetUserData(userCd);

            if (userData.Coins < totalCoins)
            {
                return JsonSerializer.Serialize(new Dictionary<string, string> { { "status", "400" }, { "message", "コインが不足しています" } });
            }

            try
            {
                PleasanterApiResponseEntity response = await SendOrder(
                    orderPostEntityList: OrderPostEntityList,
                    sakeSiteId: sakeSiteId,
                    userCd: userCd,
                    userName: userData.UserName ?? ""
                );

                return JsonSerialize(response);
            }
            catch (HttpRequestException ex)
            {
                return ex.ToString();
            }
        }

        private async Task<PleasanterApiResponseEntity> SendOrder(
            List<OrderPostEntity> orderPostEntityList,
            int sakeSiteId,
            string userCd,
            string userName
        )
        {
            List<SendOrderPurchaseEntity> sendOrderPurchaseEntityList = new List<SendOrderPurchaseEntity>();
            int orderResultId = 0;

            #if DEBUG
                int extraId = 1014;
            #else
                int extraId = 56483;
            #endif

            List<OrderPostEntity> alcoholOrderPostEntityList = orderPostEntityList.Where(x => x.Category == 2).ToList();
            List<OrderPostEntity> nonAlcoholOrderPostEntityList = orderPostEntityList.Where(x => x.Category != 2).ToList();

            // アルコールの登録
            if (alcoholOrderPostEntityList.Count() > 0 || orderPostEntityList.Any(x => x.ResultId == extraId))
            {
                orderResultId = await SendOrder(
                    orderPostEntity: alcoholOrderPostEntityList.FirstOrDefault()!,
                    userCd :userCd,
                    coins: alcoholOrderPostEntityList.Sum(x => x.OrderCoins),
                    isAlcohol: true
                );
            }

            if (orderPostEntityList.Any(x => x.ResultId == extraId))
            {
                // extraリストを分離してアルコールを登録
                OrderPostEntity extraOrderPostEntity = OrderPostEntityList.Where(x => x.ResultId == extraId).FirstOrDefault()!;
                SetAlcoholDataEntityProcess setAlcoholDataEntityProcess = new(sakeSiteId, OrderSiteId, EventId, userCd);
                List<int> extraAlcoholIdList = setAlcoholDataEntityProcess.GetHidedIdList();

                foreach (int extraAlcoholId in extraAlcoholIdList)
                {
                    sendOrderPurchaseEntityList.Add(
                        new SendOrderPurchaseEntity
                        (
                            eventNumber: EventId,
                            orderResultId: orderResultId.ToString(),
                            userCd: userCd,
                            purchaseResultId: extraAlcoholId.ToString(),
                            orderNumber: extraOrderPostEntity.OrderNumber,
                            orderCoins: extraOrderPostEntity.OrderCoins,
                            alcoholAmount: extraOrderPostEntity.AlcoholAmount,
                            isAlcohol: true
                        )
                    );
                }
            }

            sendOrderPurchaseEntityList.AddRange(
                CreateSendOrderPurchaseEntities(
                    orderPostEntityList: alcoholOrderPostEntityList,
                    orderResultId: orderResultId.ToString(),
                    userCd: userCd,
                    isAlcohol: true
                )
            );

            // アルコール以外の登録
            if (nonAlcoholOrderPostEntityList.Count() > 0)
            {
                orderResultId = await SendOrder(
                    orderPostEntity: nonAlcoholOrderPostEntityList.FirstOrDefault()!,
                    userCd: userCd,
                    coins: alcoholOrderPostEntityList.Sum(x => x.OrderCoins),
                    isAlcohol: false
                );
            }

            sendOrderPurchaseEntityList.AddRange(
                CreateSendOrderPurchaseEntities(
                    orderPostEntityList: nonAlcoholOrderPostEntityList,
                    orderResultId: orderResultId.ToString(),
                    userCd: userCd,
                    isAlcohol: false
                )
            );

            // 印刷(表示)用リストの作成
            List<OrderTableEntity> orderTableEntityList = CreateBackOrderTableEntityList(
                sendOrderPurchaseEntityList: sendOrderPurchaseEntityList,
                orderResultId: orderResultId,
                userCd: userCd,
                userName: userName
            );

            PleasanterApi pleasanterApi = new();

            try
            {
                PleasanterApiResponseEntity response = await pleasanterApi.BulkInsert(sendOrderPurchaseEntityList, OrderSiteId.ToString());
                ErrorCheck(response);

                // 正常終了時、OrderListに通知
                await _hubContext.Clients.All.SendAsync("NewOrder", new Dictionary<string, string>() {
                    { "userName", userName },
                    { "orderTableEntityList", JsonSerializer.Serialize(orderTableEntityList) }
                });

                return response;
            }
            catch (HttpRequestException)
            {
                throw;
            }
        }

        private async Task<int> SendOrder(OrderPostEntity orderPostEntity, string userCd, int coins, bool isAlcohol)
        {   
            SendOrderEntity sendOrderEntity = new(
                eventNumber: EventId,
                userCd: userCd,
                orderCoins: coins,
                isAlcohol: isAlcohol
            );
            PleasanterApi pleasanterApi = new();
            PleasanterApiResponseEntity response = await pleasanterApi.Insert(sendOrderEntity, OrderSiteId.ToString());
            try
            {
                ErrorCheck(response);
            }catch (HttpRequestException)
            {
                throw;
            }

            return response.Id;
        }

        private List<SendOrderPurchaseEntity> CreateSendOrderPurchaseEntities(
            List<OrderPostEntity> orderPostEntityList,
            string orderResultId,
            string userCd,
            bool isAlcohol
        )
        {
            return orderPostEntityList.Select(orderPostEntity =>
                new SendOrderPurchaseEntity(
                    eventNumber: EventId,
                    orderResultId: orderResultId,
                    userCd: userCd,
                    purchaseResultId: orderPostEntity.ResultId.ToString(),
                    orderNumber: orderPostEntity.OrderNumber,
                    orderCoins: orderPostEntity.OrderCoins,
                    alcoholAmount: orderPostEntity.AlcoholAmount,
                    isAlcohol: isAlcohol
                )
            ).ToList();
        }

        private List<OrderTableEntity> CreateBackOrderTableEntityList(
            List<SendOrderPurchaseEntity> sendOrderPurchaseEntityList,
            int orderResultId,
            string userCd,
            string userName
        )
        {
            List<OrderTableEntity> orderTableEntity = new List<OrderTableEntity>();

            ResultsEntity? resultsEntity = String.IsNullOrEmpty(orderResultId.ToString()) ? null : base.GetSingleRecord(orderResultId.ToString());

            string? orderNumber = resultsEntity?.ClassC?.ToString();

            string[] orderNumbers = string.IsNullOrEmpty(orderNumber) ? [] : orderNumber.Split('-');

            if (orderNumbers.Length >= 4)
            {
                orderNumber = $"{orderNumbers[2]}-{orderNumbers[3]}";
            }

            List<List<SendOrderPurchaseEntity>> separateSendOrderPurchaseEntityList = new List<List<SendOrderPurchaseEntity>>()
            {
                sendOrderPurchaseEntityList.Where(x => x.CheckHash.CheckA).ToList(),
                sendOrderPurchaseEntityList.Where(x => !x.CheckHash.CheckA).ToList()
            };

            separateSendOrderPurchaseEntityList.ForEach(separateSendOrderPurchaseEntity =>
            {
                orderTableEntity.Add(
                    new OrderTableEntity(
                        resultId: resultsEntity?.ResultId.ToString(),
                        userCd: userCd,
                        userName: userName,
                        orderNumber: orderNumber,
                        isAlcohol: separateSendOrderPurchaseEntity[0].CheckHash.CheckA,
                        orderPurchaseTableEntityList: CreateBackOrderPurchaseTableEntity(
                            sendOrderPurchaseEntityList: sendOrderPurchaseEntityList,
                            userCd: userCd,
                            userName: userName,
                            isAlcohol: separateSendOrderPurchaseEntity[0].CheckHash.CheckA
                        )
                    )
                );
            });

            return orderTableEntity;
        }

        private List<OrderPurchaseTableEntity> CreateBackOrderPurchaseTableEntity(
            List<SendOrderPurchaseEntity> sendOrderPurchaseEntityList,
            string userCd,
            string userName,
            bool isAlcohol
        )
        {
            List<OrderPurchaseTableEntity> orderPurchaseTableEntity = new List<OrderPurchaseTableEntity>();

            sendOrderPurchaseEntityList.ForEach(sendOrderPurchaseEntity =>
            {
                ResultsEntity? resultsEntity = String.IsNullOrEmpty(sendOrderPurchaseEntity.ClassHash.ClassC.ToString()) ? null : base.GetSingleRecord(sendOrderPurchaseEntity.ClassHash.ClassC.ToString());

                orderPurchaseTableEntity
                    .Add(
                        new OrderPurchaseTableEntity(
                            resultId: "",
                            userCd: userCd,
                            userName: userName,
                            purchaseCd: sendOrderPurchaseEntity.ClassHash.ClassC.ToString(),
                            purchaseName: resultsEntity?.ClassA,
                            purchaseRead: resultsEntity?.ClassJ,
                            number: sendOrderPurchaseEntity.NumHash.NumA,
                            unit: "",
                            coins: sendOrderPurchaseEntity.NumHash.NumB,
                            alcoholQuantity: sendOrderPurchaseEntity.NumHash.NumC,
                            isAlcohol: isAlcohol,
                            hide: resultsEntity?.CheckZ ?? false
                        )
                    );
            });

            return orderPurchaseTableEntity;
        }

        private string JsonSerialize(PleasanterApiResponseEntity response)
        {
            return JsonSerializer.Serialize(new Dictionary<string, string> {
                { "status", response.StatusCode.ToString() },
                { "message", response.Message.ToString() },
                { "targeturl", response.Url ?? "" },
                { "sendData", response.SendData ?? "" }
            });
        }

        private void ErrorCheck(PleasanterApiResponseEntity response)
        {
            if (!response.StatusCode.Equals(HttpStatusCode.OK))
            {
                throw new HttpRequestException(
                    JsonSerialize(response)
                );
            }
        }
    }
}