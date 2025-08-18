using offlineMeeting.Models.Entity.Pos;
using offlineMeeting.Models.Share;
using System.Data;
using System.Text.Json;

namespace offlineMeeting.Models.Process.Pos
{
    public class SendOrderProcess
    {
        private int OrderSiteId;
        private string EventId;
        private List<OrderPostEntity> OrderPostEntityList;

        public SendOrderProcess(List<OrderPostEntity> orderPostEntityList, int orderSiteId, HttpContext httpContext)
        {
            ManageEventId manageEventId = new ManageEventId(httpContext);

            EventId = manageEventId.GetEventId();
            if (EventId == "0")
            {
                throw new DataException("イベントIDが不正です");
            }
            OrderSiteId = orderSiteId;
            OrderPostEntityList = orderPostEntityList.OrderBy(x => x.ResultId).ToList();
        }


        public async Task<bool> Send(HttpContext httpContext)
        {
            List<SendRegisterEntity> sendOrderEntityList = new List<SendRegisterEntity>();
            ManageUserCd manageUserCd = new ManageUserCd(httpContext);
            string userCd = manageUserCd.GetUserCd().ToString();
            if(userCd == "0")
            {
                return false;
            }

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
            }

            SendPlesanterApiEntity sendPlesanterApiEntity = new(data: sendOrderEntityList);

            string json = JsonSerializer.Serialize(sendPlesanterApiEntity);

            HttpClient httpClient = new HttpClient();
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await httpClient.PostAsync($"http://localhost:59802/api/items/{OrderSiteId}/bulkupsert", content);

            return true;
        }
    }
}