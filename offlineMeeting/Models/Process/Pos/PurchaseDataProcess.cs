using offlineMeeting.Models.Entity.Pos;
using offlineMeeting.Models.Process.Share;
using offlineMeeting.Models.Share;
using System.Data;

namespace offlineMeeting.Models.Process.Pos
{
    public class PurchaseDataProcess
    {
        private int ExtraSiteId;
        private int EventFoodSiteId;
        private int RawFoorSiteId;
        private string EventId;

        public PurchaseDataProcess(
            int extraSiteId,
            int eventFoodSiteId,
            int rawFoorSiteId,
            HttpContext httpContext)
        {
            ManageEventId manageEventId = new (httpContext);

            ExtraSiteId = extraSiteId;
            EventFoodSiteId = eventFoodSiteId;
            RawFoorSiteId = rawFoorSiteId;
            EventId = manageEventId.GetEventId();
            if(EventId == "0")
            {
                CreateLogProcess.Error("POS_016", "イベントIDが不正です");
                throw new DataException("イベントIDが不正です");
            }
        }

        public List<PurchaseDataEntity> Get(HttpContext httpContext, int orderSiteId, int category)
        {
            ManageUserCd manageUserCd = new ManageUserCd(httpContext);
            string userCd = manageUserCd.GetUserCd().ToString();

            SetPurchaseDataEntityListProcess setPurchaseDataEntityListProcess = new (
                ExtraSiteId,
                EventFoodSiteId,
                RawFoorSiteId,
                orderSiteId,
                EventId,
                userCd
            );
            return setPurchaseDataEntityListProcess
                .Get(category)
                .ToList();
        }
    }
}