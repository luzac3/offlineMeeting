using offlineMeeting.Models.Entity.Coins;
using offlineMeeting.Models.Share;
using PleasanterBridge.src.APIBridge;
using System.Net;

namespace offlineMeeting.Models.Process.Coins
{
    public class SendCoinsProcess
    {
        private readonly IPleasanterApiBridge _bridge;
        private CoinsEntity CoinsEntity;
        private int CoinsSiteId;
        private int EventId;
        private int UserCd;

        public SendCoinsProcess(HttpContext httpContext, IPleasanterApiBridge bridge, CoinsEntity coinsEntity, int coinsSiteId)
        {
            _bridge = bridge;
            ManageEventId manageEventId = new ManageEventId(httpContext);
            ManageUserCd manageUserCd = new ManageUserCd(httpContext);
            CoinsEntity = coinsEntity;
            CoinsSiteId = coinsSiteId;
            EventId = manageEventId.GetEventId();
            UserCd = manageUserCd.GetUserCd();
        }

        public async Task<bool> Send()
        {
            SendCoinsEntity sendCoinsEntity = new SendCoinsEntity
            (
                eventNumber: EventId.ToString(),
                userCd: UserCd.ToString(),
                gar: "100",
                coins: CoinsEntity.Coins,
                value: CoinsEntity.Value
            );
            // 課金の場合、ユーザがいなかったらUndefinedUserとして課金情報だけは保存する必要がある
            // これはイベントIDも同じ
            // なのでIDのチェックはしない
            var response = await _bridge.Insert(CoinsSiteId, sendCoinsEntity);

            return response.StatusCode.Equals(HttpStatusCode.OK);
        }
    }
}