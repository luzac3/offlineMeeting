using Microsoft.AspNetCore.SignalR;
using offlineMeeting.Models.Entity.Login;
using offlineMeeting.Models.Entity.Pos;
using offlineMeeting.Models.Entity.Share;
using offlineMeeting.Models.Process.Share;
using offlineMeeting.Models.Share;
using System.Data;
using System.Net;
using System.Text.Json;

namespace offlineMeeting.Models.Process.Pos
{
    public class SendOrderProcess
    {
        private int OrderSiteId;
        private int JoinerSiteId;
        private string EventId;
        private HttpContext HttpContext;
        private List<OrderPostEntity> OrderPostEntityList;
        private readonly IHubContext<PosHub> _hubContext;

        public SendOrderProcess(List<OrderPostEntity> orderPostEntityList, int orderSiteId, int joinerSiteId, HttpContext httpContext, IHubContext<PosHub> hubContext)
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


        public async Task<string> Send()
        {
            #if DEBUG
                string pleasanterUrlBase = "http://localhost:59802";
            #else
                string pleasanterUrlBase = "https://pleasanter.wolfslab.jp";
            #endif
            List<SendOrderEntity> sendOrderEntityList = new List<SendOrderEntity>();
            ManageUserCd manageUserCd = new ManageUserCd(HttpContext);
            string userCd = manageUserCd.GetUserCd().ToString();
            if (userCd == "0")
            {
                return JsonSerializer.Serialize(new Dictionary<string, string> { { "status", "400" }, { "message", "ユーザコードがありません" } });
            }

            // コインが足りることを確認する
            int totalCoins = 0;
                
            foreach (OrderPostEntity orderPostEntity in OrderPostEntityList)
            {
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
            }

            GetUsersProcess getUsersProcess = new (JoinerSiteId, HttpContext);
            JoinerEntity userData = getUsersProcess.GetUserData(userCd);

            if(userData.Coins < totalCoins)
            {
                return JsonSerializer.Serialize(new Dictionary<string, string> { { "status", "400" }, { "message", "コインが不足しています" } });
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
                await _hubContext.Clients.All.SendAsync("NewOrder", new Dictionary<string, string>() { {"userName", userData.UserName ?? ""} });

                return JsonSerializer.Serialize(new Dictionary<string, string> { { "status", "200" }, { "message", "注文を送信しました" }, { "url", url }, { "userCd", userCd }, { "sendData", json } });
            }
        }
    }
}