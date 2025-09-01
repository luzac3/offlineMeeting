using offlineMeeting.Models.Entity.Share;
using offlineMeeting.Models.Process.Share;
using offlineMeeting.Models.Share;
using System.Data;

namespace offlineMeeting.Models.Process.Collection
{
    public class AlcoholDataProcess
    {
        private int SakeSiteId;
        private int OrderSiteId;
        private string EventId;
        private HttpContext HttpContext;

        public AlcoholDataProcess(int sakeSiteId, int orderSiteId, HttpContext httpContext)
        {
            ManageEventId manageEventId = new (httpContext);

            SakeSiteId = sakeSiteId;
            OrderSiteId = orderSiteId;
            EventId = manageEventId.GetEventId();
            if(EventId == "0")
            {
                throw new DataException("イベントIDが不正です");
            }
            HttpContext = httpContext;
        }

        public List<AlcoholDataEntity> Get(System.Text.Json.Nodes.JsonNode? json)
        {
            ManageUserCd manageUserCd = new ManageUserCd(HttpContext);
            string userCd = manageUserCd.GetUserCd().ToString();

            SetAlcoholDataEntityProcess setAlcoholDataEntityProcess = new (SakeSiteId, OrderSiteId, EventId, userCd);
            return setAlcoholDataEntityProcess
                .Get(json)
                .OrderBy(x=>x.Rarity)
                .ToList();
        }

        public AlcoholDataEntity Get(System.Text.Json.Nodes.JsonNode? json, int resultId)
        {
            ManageUserCd manageUserCd = new ManageUserCd(HttpContext);
            string userCd = manageUserCd.GetUserCd().ToString();

            SetAlcoholDataEntityProcess setAlcoholDataEntityProcess = new(SakeSiteId, OrderSiteId, EventId, userCd);
            return setAlcoholDataEntityProcess.Get(json, resultId);
        }
    }
}