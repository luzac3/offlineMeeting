using offlineMeeting.Models.Entity.Share;
using offlineMeeting.Models.Process.Share;
using offlineMeeting.Models.Share;
using PleasanterBridge.src.DataRepository.Service;
using System.Data;

namespace offlineMeeting.Models.Process.Pos
{
    public class AlcoholDataProcess
    {
        private int SakeSiteId;
        private int EventId;

        public AlcoholDataProcess(int sakeSiteId, HttpContext httpContext)
        {
            ManageEventId manageEventId = new (httpContext);

            SakeSiteId = sakeSiteId;
            EventId = manageEventId.GetEventId();
        }

        public List<AlcoholDataEntity> Get(System.Text.Json.Nodes.JsonNode? json, HttpContext httpContext, IPleasanterRepository repository, int orderSiteId)
        {
            int userCd = new ManageUserCd(httpContext).GetUserCd();

            SetAlcoholDataEntityProcess setAlcoholDataEntityProcess = new (repository, SakeSiteId, orderSiteId, EventId, userCd);
            return setAlcoholDataEntityProcess
                .Get(json)
                .Where(x => !x.Hide)
                .OrderByDescending(x => x.Priority)
                .ThenBy(x=>x.Rarity)
                .ToList();
        }
    }
}