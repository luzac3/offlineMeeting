using offlineMeeting.Models.Entity.Coins;
using offlineMeeting.Models.Share;
using System.Net;
using System.Text.Json;

namespace offlineMeeting.Models.Process.Coins
{
    public class GiveCoinsProcess
    {
        private GiveCoinsEntity GiveCoinsEntity;
        private int CoinsSiteId;
        private string EventId;
        private string UserCd;

        public GiveCoinsProcess(GiveCoinsEntity giveCoinsEntity, int coinsSiteId, HttpContext httpContext)
        {
            ManageEventId manageEventId = new ManageEventId(httpContext);
            ManageUserCd manageUserCd = new ManageUserCd(httpContext);
            GiveCoinsEntity = giveCoinsEntity;
            CoinsSiteId = coinsSiteId;
            EventId = manageEventId.GetEventId();
            UserCd = manageUserCd.GetUserCd().ToString();
        }

        public async Task<bool> Send()
        {
            HttpClient httpClient = new();

            #if DEBUG
                string pleasanterUrlBase = "http://localhost:59802";
            #else
                string pleasanterUrlBase = "https://pleasanter.wolfslab.jp";
            #endif

            SendCoinsEntity sendCoinsEntity = new
            (
                eventNumber: EventId,
                userCd: UserCd,
                gar: "150",
                coins: GiveCoinsEntity.Coins,
                value: GiveCoinsEntity.Value
            );

            string json = JsonSerializer.Serialize(sendCoinsEntity);
            StringContent content = new (json, System.Text.Encoding.UTF8, "application/json");

            var responseGive = await httpClient.PostAsync($"{pleasanterUrlBase}/api/items/{CoinsSiteId}/create", content);

            if (responseGive.StatusCode.Equals(HttpStatusCode.OK))
            {
                throw new Exception("give coins failue");
            }

            sendCoinsEntity = new
            (
                eventNumber: EventId,
                userCd: GiveCoinsEntity.forUserCd,
                gar: "250",
                coins: GiveCoinsEntity.Coins,
                value: GiveCoinsEntity.Value
            );

            json = JsonSerializer.Serialize(sendCoinsEntity);
            content = new(json, System.Text.Encoding.UTF8, "application/json");

            var responseReceive = await httpClient.PostAsync($"{pleasanterUrlBase}/api/items/{CoinsSiteId}/create", content);

            if (responseReceive.StatusCode.Equals(HttpStatusCode.OK))
            {
                throw new Exception("receive coins failue");
            }

            // あるいはQRで譲渡にすべきだったかもしれねえ

            return responseReceive.StatusCode.Equals(HttpStatusCode.OK);
        }
    }
}