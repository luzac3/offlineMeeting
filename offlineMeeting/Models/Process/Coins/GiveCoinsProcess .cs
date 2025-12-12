using offlineMeeting.Models.Entity.Coins;
using offlineMeeting.Models.Share;
using System.Net;
using System.Text.Json;
using PleasanterBridge.src.APIBridge;

namespace offlineMeeting.Models.Process.Coins
{
    public class GiveCoinsProcess
    {
        private GiveCoinsEntity GiveCoinsEntity;
        private int CoinsSiteId;
        private string EventId;
        private string UserCd;
        private readonly IPleasanterApiBridge _bridge;

        public GiveCoinsProcess(GiveCoinsEntity giveCoinsEntity, int coinsSiteId, HttpContext httpContext, IPleasanterApiBridge bridge)
        {
            ManageEventId manageEventId = new ManageEventId(httpContext);
            ManageUserCd manageUserCd = new ManageUserCd(httpContext);
            GiveCoinsEntity = giveCoinsEntity;
            CoinsSiteId = coinsSiteId;
            EventId = manageEventId.GetEventId();
            UserCd = manageUserCd.GetUserCd().ToString();
            _bridge = bridge;
        }

        public async Task<Dictionary<string, string>> Send()
        {
            var responseGive = await SendCoinsAndGetResponse(
                userCd: UserCd,
                gar: "250",
                coins: GiveCoinsEntity.Coins * -1
            );

            if (!responseGive.StatusCode.Equals(HttpStatusCode.OK))
            {
                return new Dictionary<string, string> {
                    {"StatusCode", responseGive.StatusCode.ToString() },
                    {"Message", "receive coins failue"}
                };
                throw new Exception("give coins failue");
            }

            int coinsRate = (int)Math.Round(GiveCoinsEntity.Coins * 0.2) == 0 ? 1 : (int)Math.Round(GiveCoinsEntity.Coins * 0.2);

            var responseReceive = await SendCoinsAndGetResponse(
                userCd: GiveCoinsEntity.ForUserCd.ToString(),
                gar: "150",
                coins: (int)GiveCoinsEntity.Coins - coinsRate
            );

            if (!responseReceive.StatusCode.Equals(HttpStatusCode.OK))
            {
                return new Dictionary<string, string> {
                    {"StatusCode", responseReceive.StatusCode.ToString() },
                    {"Message", "receive coins failue"}
                };
                throw new Exception("receive coins failue");
            }

            return new Dictionary<string, string> {
                    {"StatusCode", responseReceive.StatusCode.ToString() },
                    {"Message", "receive coins success"}
                };
        }

        private async Task<dynamic> SendCoinsAndGetResponse(string userCd, string gar, int coins)
        {
            var sendCoinsEntity = new SendCoinsEntity
            (
                eventNumber: EventId,
                userCd: userCd,
                gar: gar,
                coins: coins,
                value: GiveCoinsEntity.Value
            );

            return await _bridge.Insert(CoinsSiteId, sendCoinsEntity);
        }
    }
}