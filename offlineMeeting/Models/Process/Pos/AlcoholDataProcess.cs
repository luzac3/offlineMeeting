using offlineMeeting.Models.Entity.Share;
using offlineMeeting.Models.Process.Share;
using offlineMeeting.Models.Share;
using PleasanterBridge.src.DataRepository.Service;
using System.Data;

namespace offlineMeeting.Models.Process.Pos
{
    public class AlcoholDataProcess
    {
        private long SakeSiteId;
        private long EventId;

        public AlcoholDataProcess(long sakeSiteId, HttpContext httpContext)
        {
            ManageEventId manageEventId = new(httpContext);

            SakeSiteId = sakeSiteId;
            EventId = manageEventId.GetEventId();
        }

        public List<AlcoholDataEntity> Get(System.Text.Json.Nodes.JsonNode? json, HttpContext httpContext, IPleasanterRepository repository, long orderSiteId, IWebHostEnvironment? env = null)
        {
            long userCd = new ManageUserCd(httpContext).GetUserCd();

            SetAlcoholDataEntityProcess setAlcoholDataEntityProcess = new(repository, SakeSiteId, orderSiteId, EventId, userCd, env);
            return setAlcoholDataEntityProcess
                .Get(json)
                .Where(x => !x.Hide)
                .OrderByDescending(x => x.Priority)
                .ThenBy(x => x.Rarity)
                .ToList();
        }
    }
}