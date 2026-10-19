using Microsoft.AspNetCore.Hosting;
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
        private long SakeSiteId;
        private long OrderSiteId;
        private long EventId;
        private long UserCd;
        private IWebHostEnvironment? _env;

        public AlcoholDataProcess(HttpContext httpContext, IPleasanterRepository pleasanterRepository, long sakeSiteId, long orderSiteId, IWebHostEnvironment? env = null)
        {
            _pleasanterRepository = pleasanterRepository;
            UserCd = new ManageUserCd(httpContext).GetUserCd();
            ManageEventId manageEventId = new (httpContext);

            SakeSiteId = sakeSiteId;
            OrderSiteId = orderSiteId;
            EventId = manageEventId.GetEventId();
            HttpContext = httpContext;
            _env = env;
        }

        public List<AlcoholDataEntity> Get(System.Text.Json.Nodes.JsonNode? json)
        {
            SetAlcoholDataEntityProcess setAlcoholDataEntityProcess = new (_pleasanterRepository, SakeSiteId, OrderSiteId, EventId, UserCd, _env);
            return setAlcoholDataEntityProcess
                .Get(json)
                .OrderBy(x=>x.Rarity)
                .ToList();
        }

        public AlcoholDataEntity Get(System.Text.Json.Nodes.JsonNode? json, long resultId)
        {
            SetAlcoholDataEntityProcess setAlcoholDataEntityProcess = new(_pleasanterRepository, SakeSiteId, OrderSiteId, EventId, UserCd, _env);
            return setAlcoholDataEntityProcess.Get(json, resultId);
        }
    }
}