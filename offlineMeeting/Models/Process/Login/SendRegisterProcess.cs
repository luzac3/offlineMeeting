using offlineMeeting.Models.Entity.Pos;
using offlineMeeting.Models.Share;
using System.Text.Json;

namespace offlineMeeting.Models.Process.Login
{
    public class SendRegisterProcess
    {
        public string UserCd;
        public SendRegisterProcess(string userCd) { 
            UserCd = userCd;
        }

        public async Task<bool> Send()
        {
            SendRegisterEntity sendRegisterEntity = new(
                
            );

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
