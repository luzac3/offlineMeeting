using offlineMeeting.Models.Entity.Coins;
using offlineMeeting.Models.Process.Share;
using offlineMeeting.Models.Share;
using System.Net;
using System.Text.Json;

namespace offlineMeeting.Models.Process.Coins
{
    public class SendCoinsProcess
    {
        private CoinsEntity CoinsEntity;
        private int CoinsSiteId;
        private string EventId;
        private string UserCd;

        public SendCoinsProcess(CoinsEntity coinsEntity, int coinsSiteId, HttpContext httpContext)
        {
            ManageEventId manageEventId = new ManageEventId(httpContext);
            ManageUserCd manageUserCd = new ManageUserCd(httpContext);
            CoinsEntity = coinsEntity;
            CoinsSiteId = coinsSiteId;
            EventId = manageEventId.GetEventId();
            UserCd = manageUserCd.GetUserCd().ToString();
        }

        public async Task<bool> Send()
        {
            #if DEBUG
                string pleasanterUrlBase = "http://localhost:59802";
            #else
                string pleasanterUrlBase = "https://pleasanter.wolfslab.jp";
            #endif
            SendCoinsEntity sendCoinsEntity = new SendCoinsEntity
            (
                eventNumber: EventId,
                userCd: UserCd,
                gar: "100",
                coins: CoinsEntity.Coins,
                value: CoinsEntity.Value
            );
            // 課金の場合、ユーザがいなかったらUndefinedUserとして課金情報だけは保存する必要がある
            // これはイベントIDも同じ
            // なのでIDのチェックはしない

            string json = JsonSerializer.Serialize(sendCoinsEntity);
            StringContent content = new (json, System.Text.Encoding.UTF8, "application/json");

            HttpClient httpClient = new ();
            var response = await httpClient.PostAsync($"{pleasanterUrlBase}/api/items/{CoinsSiteId}/create", content);

            if (!response.StatusCode.Equals(HttpStatusCode.OK))
            {
                CreateLogProcess.Error("COINS_006", $"send coins failure StatusCode={response.StatusCode}");
            }

            return response.StatusCode.Equals(HttpStatusCode.OK);
        }
    }
}