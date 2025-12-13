using offlineMeeting.Models.Entity.Share;
using offlineMeeting.Models.Process.Share;
using offlineMeeting.Models.Share;
using PleasanterBridge.src.DataRepository.Service;
using System.Data;

namespace offlineMeeting.Models.Process.Collection
{
    public class AlcoholDataProcess
    {
        private HttpContext HttpContext;
        private IPleasanterRepository _pleasanterRepository;
        private int SakeSiteId;
        private int OrderSiteId;
        private int EventId;
        private int UserCd;

        public AlcoholDataProcess(HttpContext httpContext, IPleasanterRepository pleasanterRepository, int sakeSiteId, int orderSiteId)
        {
            _pleasanterRepository = pleasanterRepository;
            UserCd = new ManageUserCd(httpContext).GetUserCd();
            ManageEventId manageEventId = new (httpContext);

            SakeSiteId = sakeSiteId;
            OrderSiteId = orderSiteId;
            EventId = manageEventId.GetEventId();
            HttpContext = httpContext;
        }

        public List<AlcoholDataEntity> Get(System.Text.Json.Nodes.JsonNode? json)
        {
            SetAlcoholDataEntityProcess setAlcoholDataEntityProcess = new (_pleasanterRepository, SakeSiteId, OrderSiteId, EventId, UserCd);
            return setAlcoholDataEntityProcess
                .Get(json)
                .OrderBy(x=>x.Rarity)
                .ToList();
        }

        public AlcoholDataEntity Get(System.Text.Json.Nodes.JsonNode? json, int resultId)
        {
            SetAlcoholDataEntityProcess setAlcoholDataEntityProcess = new(_pleasanterRepository, SakeSiteId, OrderSiteId, EventId, UserCd);
            return setAlcoholDataEntityProcess.Get(json, resultId);
        }
    }
}