using offlineMeeting.Models.Entity.Share;
using offlineMeeting.Models.Process.Share;
using offlineMeeting.Models.Share;
using System.Data;

namespace offlineMeeting.Models.Process.Pos
{
    public class AlcoholDataProcess
    {
        private int SakeSiteId;
        private string EventId;

        public AlcoholDataProcess(int sakeSiteId, HttpContext httpContext)
        {
            ManageEventId manageEventId = new (httpContext);

            SakeSiteId = sakeSiteId;
            EventId = manageEventId.GetEventId();
            if(EventId == "0")
            {
                throw new DataException("イベントIDが不正です");
            }
        }

        public List<AlcoholDataEntity> Get(System.Text.Json.Nodes.JsonNode? json, HttpContext httpContext, int orderSiteId)
        {
            ManageUserCd manageUserCd = new ManageUserCd(httpContext);
            string userCd = manageUserCd.GetUserCd().ToString();

            SetAlcoholDataEntityProcess setAlcoholDataEntityProcess = new (SakeSiteId, orderSiteId, EventId, userCd);
            return setAlcoholDataEntityProcess
                .Get(json)
                .OrderBy(x=>x.Rarity)
                .ToList();
        }
    }
}