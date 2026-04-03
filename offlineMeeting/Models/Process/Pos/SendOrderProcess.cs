using Microsoft.AspNetCore.SignalR;
using offlineMeeting.Models.DBProperty;
using offlineMeeting.Models.Entity.Login;
using offlineMeeting.Models.Entity.Pos;
using offlineMeeting.Models.Entity.Pos.SendOrderEntity;
using offlineMeeting.Models.Entity.Pos.SendOrderPurchaseEntity;
using offlineMeeting.Models.Entity.Share;
using offlineMeeting.Models.Process.Share;
using offlineMeeting.Models.Share;
using PleasanterBridge.src.DataRepository.Entity;
using PleasanterBridge.src.DataRepository.Service;
using System.Data;
using System.Net;
using System.Text.Json;

namespace offlineMeeting.Models.Process.Pos
{
    public class SendOrderProcess
    {
        private long OrderSiteId;
        private long OrderPurchaseSiteId;
        private long JoinerSiteId;
        private long EventId;
        private HttpContext HttpContext;
        private List<OrderPostEntity> OrderPostEntityList;
        private readonly IHubContext<MyHub> _hubContext;
        private readonly IPleasanterRepository _pleasanterRepository;

        public SendOrderProcess(
            List<OrderPostEntity> orderPostEntityList,
            long orderSiteId,
            long orderPurchseSiteId,
            long joinerSiteId,
            HttpContext httpContext, 
            IHubContext<MyHub> hubContext,
            IPleasanterRepository pleasanterRepository
        )
        {
            ManageEventId manageEventId = new ManageEventId(httpContext);

            EventId = manageEventId.GetEventId();

            OrderSiteId = orderSiteId;
            OrderPurchaseSiteId = orderPurchseSiteId;
            JoinerSiteId = joinerSiteId;
            HttpContext = httpContext;
            OrderPostEntityList = orderPostEntityList.OrderBy(x => x.ResultId).ToList();
            _hubContext = hubContext;
            _pleasanterRepository = pleasanterRepository;
        }


        public async Task<string> Send(long sakeSiteId)
        {
            List<SendOrderPurchaseEntity> sendOrderPurchaseEntityList = new List<SendOrderPurchaseEntity>();
            List<OrderPurchaseTableEntity> orderPurchaseTableEntity = new List<OrderPurchaseTableEntity>();
            ManageUserCd manageUserCd = new ManageUserCd(HttpContext);
            long userCd = manageUserCd.GetUserCd();
            if (userCd == 0)
            {
                return JsonSerializer.Serialize(new Dictionary<string, string> { { "status", "400" }, { "message", "ユーザコードがありません" } });
            }

            // コインが足りることを確認する
            int totalCoins = 0;

            GetUsersProcess getUsersProcess = new(HttpContext, _pleasanterRepository, JoinerSiteId);
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
            long sakeSiteId,
            long userCd,
            string userName
        )
        {
            List<SendOrderPurchaseEntity> sendOrderPurchaseEntityList = new List<SendOrderPurchaseEntity>();
            long orderResultId = 0;
            List<long> orderResultIdList = new ();

            List<OrderPostEntity> alcoholOrderPostEntityList = orderPostEntityList.Where(x => x.Category == 2).ToList();
            List<OrderPostEntity> nonAlcoholOrderPostEntityList = orderPostEntityList.Where(x => x.Category != 2).ToList();

            // アルコールの登録
            if (alcoholOrderPostEntityList.Count() > 0)
            {
                orderResultId = await SendOrder(
                    orderPostEntity: alcoholOrderPostEntityList.FirstOrDefault()!,
                    userCd :userCd,
                    coins: alcoholOrderPostEntityList.Sum(x => x.OrderCoins),
                    isAlcohol: true
                );
                orderResultIdList.Add(orderResultId);
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
                    coins: nonAlcoholOrderPostEntityList.Sum(x => x.OrderCoins),
                    isAlcohol: false
                );
                orderResultIdList.Add(orderResultId);
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
                orderResultIdList: orderResultIdList,
                userCd: userCd,
                userName: userName
            );

            PleasanterApi pleasanterApi = new();

            try
            {
                PleasanterApiResponseEntity response = await pleasanterApi.BulkInsert(sendOrderPurchaseEntityList, OrderPurchaseSiteId.ToString());
                ErrorCheck(response);

                // 正常終了時、OrderListに通知
                await _hubContext.Clients.Group($"event_{EventId}").SendAsync("NewOrder", new Dictionary<string, string>() {
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

        private async Task<long> SendOrder(OrderPostEntity orderPostEntity, long userCd, int coins, bool isAlcohol)
        {   
            SendOrderEntity sendOrderEntity = new(
                eventNumber: EventId.ToString(),
                userCd: userCd.ToString(),
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
            long userCd,
            bool isAlcohol
        )
        {
            return orderPostEntityList.Select(orderPostEntity =>
                new SendOrderPurchaseEntity(
                    eventNumber: EventId.ToString(),
                    orderResultId: orderResultId,
                    userCd: userCd.ToString(),
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
            List<long> orderResultIdList,
            long userCd,
            string userName
        )
        {
            List<OrderTableEntity> orderTableEntityList = new List<OrderTableEntity>();

            orderResultIdList.ForEach(orderResultId =>
            {

                TablesEntity? tablesEntity = _pleasanterRepository.Select(orderResultId, true).FirstOrDefault();

                string? orderNumber = tablesEntity?.ClassC?.ToString();

                string[] orderNumbers = string.IsNullOrEmpty(orderNumber) ? [] : orderNumber.Split('-');

                if (orderNumbers.Length >= 3)
                {
                    orderNumber = $"{orderNumbers[2]}";
                }

                orderTableEntityList.Add(
                    new OrderTableEntity(
                        resultId: tablesEntity?.ReferenceId.ToString(),
                        userCd: userCd.ToString(),
                        userName: userName,
                        orderNumber: orderNumber,
                        isAlcohol: tablesEntity?.CheckB ?? false,
                        orderPurchaseTableEntityList: CreateBackOrderPurchaseTableEntity(
                            sendOrderPurchaseEntityList: sendOrderPurchaseEntityList
                                .Where(x => x.CheckA == (tablesEntity?.CheckB ?? false))
                                .ToList(),
                            userCd: userCd,
                            userName: userName,
                            isAlcohol: tablesEntity?.CheckB ?? false
                        )
                    )
                );
            });

            return orderTableEntityList;
        }

        private List<OrderPurchaseTableEntity> CreateBackOrderPurchaseTableEntity(
            List<SendOrderPurchaseEntity> sendOrderPurchaseEntityList,
            long userCd,
            string userName,
            bool isAlcohol
        )
        {
            List<OrderPurchaseTableEntity> orderPurchaseTableEntity = new List<OrderPurchaseTableEntity>();

            sendOrderPurchaseEntityList.ForEach(sendOrderPurchaseEntity =>
            {
                // purchaseId
                TablesEntity? tablesEntity = long.TryParse(sendOrderPurchaseEntity.ClassC, out long purchaseId) ? 
                    _pleasanterRepository.Select(purchaseId, true).FirstOrDefault() : 
                    null;

                orderPurchaseTableEntity
                    .Add(
                        new OrderPurchaseTableEntity(
                            resultId: "",
                            userCd: userCd.ToString(),
                            userName: userName,
                            purchaseCd: sendOrderPurchaseEntity.ClassC.ToString(),
                            purchaseName: tablesEntity?.ClassA,
                            purchaseRead: tablesEntity?.ClassJ,
                            number: sendOrderPurchaseEntity.NumA,
                            unit: "",
                            coins: sendOrderPurchaseEntity.NumB,
                            alcoholQuantity: sendOrderPurchaseEntity.NumC,
                            isAlcohol: isAlcohol,
                            hide: tablesEntity?.CheckZ ?? false
                        )
                    );
            });

            return orderPurchaseTableEntity;
        }

        private string JsonSerialize(PleasanterApiResponseEntity response)
        {
            var options = new JsonSerializerOptions
            {
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };
            return JsonSerializer.Serialize(new Dictionary<string, string> {
                { "status", response.StatusCode.ToString() },
                { "message", response.Message ?? "" },
                { "targeturl", response.Url ?? "" },
                { "sendData", response.SendData ?? "" }
            }, options);
        }

        private void ErrorCheck(PleasanterApiResponseEntity response)
        {
            if (!response.StatusCode.Equals(HttpStatusCode.OK) && response.StatusCode != 200)
            {
                throw new HttpRequestException(
                    JsonSerialize(response)
                );
            }
        }
    }
}