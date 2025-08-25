using offlineMeeting.Models.Entity.Pos;
using offlineMeeting.Models.Entity.Share;
using offlineMeeting.Models.Process.Share;
using offlineMeeting.Models.Share;
using System.Data;
using System.Text.Json;

namespace offlineMeeting.Models.Process.Pos
{
    public class SendOrderProcess
    {
        private int OrderSiteId;
        private int JoinerSiteId;
        private string EventId;
        private List<OrderPostEntity> OrderPostEntityList;

        public SendOrderProcess(List<OrderPostEntity> orderPostEntityList, int orderSiteId, int joinerSiteId, HttpContext httpContext)
        {
            ManageEventId manageEventId = new ManageEventId(httpContext);

            EventId = manageEventId.GetEventId();
            if (EventId == "0")
            {
                throw new DataException("イベントIDが不正です");
            }
            OrderSiteId = orderSiteId;
            JoinerSiteId = joinerSiteId;
            OrderPostEntityList = orderPostEntityList.OrderBy(x => x.ResultId).ToList();
        }


        public async Task<string> Send(HttpContext httpContext)
        {
            List<SendOrderEntity> sendOrderEntityList = new List<SendOrderEntity>();
            ManageUserCd manageUserCd = new ManageUserCd(httpContext);
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

            GetUsersProcess getUsersProcess = new (JoinerSiteId, httpContext);
            JoinerEntity userData = getUsersProcess.GetUserData(userCd);

            if(userData.Coins < totalCoins)
            {
                return JsonSerializer.Serialize(new Dictionary<string, string> { { "status", "400" }, { "message", "コインが不足しています" } });
            }

            BulkApiEntity sendPlesanterApiEntity = new(data: sendOrderEntityList.Cast<object>().ToList());

            string json = JsonSerializer.Serialize(sendPlesanterApiEntity);

            HttpClient httpClient = new HttpClient();
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await httpClient.PostAsync($"http://localhost:59802/api/items/{OrderSiteId}/bulkupsert", content);

            if(!response.StatusCode.Equals(200))
            {
                return JsonSerializer.Serialize(new Dictionary<string, string> { { "status", "500" }, { "message", "注文の送信に失敗しました" } });
            }
            else
            {
                return JsonSerializer.Serialize(new Dictionary<string, string> { { "status", "200" }, { "message", "注文を送信しました" } });
            }
        }
    }
}