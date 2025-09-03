using Microsoft.AspNetCore.SignalR;
using offlineMeeting.Models.DBProperty;
using offlineMeeting.Models.Entity.Login;
using offlineMeeting.Models.Entity.Pos;
using offlineMeeting.Models.Entity.Pos.SendOrderEntity;
using offlineMeeting.Models.Entity.Share;
using offlineMeeting.Models.Process.DBProcess;
using offlineMeeting.Models.Process.Share;
using offlineMeeting.Models.Share;
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
            #if DEBUG
                string pleasanterUrlBase = "http://localhost:59802";
            #else
                string pleasanterUrlBase = "https://pleasanter.wolfslab.jp";
            #endif
            List<SendOrderEntity> sendOrderEntityList = new List<SendOrderEntity>();
            List<OrderTableEntity> orderTableEntity = new List<OrderTableEntity>();
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

            foreach (OrderPostEntity orderPostEntity in OrderPostEntityList)
            {
                // extraオーダーのチェック
                // 今回はオーダーNOで管理するより仕方ない

                #if DEBUG
                    int extraId = 1014;
                #else
                    int extraId = 56483;
                #endif
                if (orderPostEntity.ResultId == extraId)
                {
                    SetAlcoholDataEntityProcess setAlcoholDataEntityProcess = new(sakeSiteId, OrderSiteId, EventId, userCd);
                    List<int> extraAlcoholIdList = setAlcoholDataEntityProcess.GetHidedIdList();
                    foreach (int extraAlcoholId in extraAlcoholIdList)
                    {
                        sendOrderEntityList.Add(
                                new SendOrderEntity
                                (
                                    eventNumber: EventId,
                                    userCd: userCd,
                                    resultId: extraAlcoholId.ToString(),
                                    orderNumber: 1,
                                    orderCoins: 0,
                                    alcoholAmount: 36
                                )
                            );
                    }

                }
                    sendOrderEntityList.Add(
                    new SendOrderEntity
                    (
                        eventNumber: EventId,
                        userCd: userCd,
                        resultId: orderPostEntity.ResultId.ToString(),
                        orderNumber: orderPostEntity.OrderNumber,
                        orderCoins: orderPostEntity.OrderCoins,
                        alcoholAmount: orderPostEntity.AlcoholAmount
                    )
                );
                totalCoins += orderPostEntity.OrderCoins;

                // 送信用
                ResultsEntity? resultsEntity = String.IsNullOrEmpty(orderPostEntity.ResultId.ToString()) ? null : base.GetSingleRecord(orderPostEntity.ResultId.ToString());

                orderTableEntity
                    .Add(
                        new OrderTableEntity(
                            resultId: "",
                            userCd: userCd,
                            userName: userData.UserName,
                            purchaseCd: orderPostEntity.ResultId.ToString(),
                            purchaseName: resultsEntity?.ClassA,
                            purchaseRead: resultsEntity?.ClassJ,
                            number: orderPostEntity.OrderNumber,
                            unit: "",
                            coins: orderPostEntity.OrderCoins,
                            alcoholQuantity: orderPostEntity.AlcoholAmount,
                            unProvided: true,
                            hide: resultsEntity?.CheckZ ?? false
                        )
                    );
            }

            BulkApiEntity sendPlesanterApiEntity = new(data: sendOrderEntityList.Cast<object>().ToList());

            string json = JsonSerializer.Serialize(sendPlesanterApiEntity);

            HttpClient httpClient = new HttpClient();
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            string url = $"{pleasanterUrlBase}/api/items/{OrderSiteId}/bulkupsert";
            HttpResponseMessage response = await httpClient.PostAsync(url, content);

            if(!response.StatusCode.Equals(HttpStatusCode.OK))
            {
                return JsonSerializer.Serialize(new Dictionary<string, string> { { "status", "500" }, { "message", "注文の送信に失敗しました" }, { "url", url }, { "sendData", json } });
            }
            else
            {
                await _hubContext.Clients.All.SendAsync("NewOrder", new Dictionary<string, string>() { {"userName", userData.UserName ?? ""}, { "orderTableEntity", JsonSerializer.Serialize(orderTableEntity) } });

                return JsonSerializer.Serialize(new Dictionary<string, string> { { "status", "200" }, { "message", "注文を送信しました" }, { "url", url }, { "userCd", userCd }, { "sendData", json } });
            }
        }
    }
}