using offlineMeeting.Models.Entity.Pos;
using offlineMeeting.Models.Share;
using System.Data;

namespace offlineMeeting.Models.Process.Pos
{
    public class ExtraDataProcess
    {
        private int RawFoodSiteId;
        private string EventId;

        public ExtraDataProcess(int rawFoodSiteId, HttpContext httpContext)
        {
            ManageEventId manageEventId = new (httpContext);

            RawFoodSiteId = rawFoodSiteId;
            EventId = manageEventId.GetEventId();
            if(EventId == "0")
            {
                throw new DataException("イベントIDが不正です");
            }
        }

        public List<FoodDataEntity> Get(HttpContext httpContext, int orderSiteId)
        {
            ManageUserCd manageUserCd = new ManageUserCd(httpContext);
            string userCd = manageUserCd.GetUserCd().ToString();

            SetFoodDataEntityListProcess setFoodDataEntityListProcess = new (RawFoodSiteId, orderSiteId, EventId, userCd);
            return setFoodDataEntityListProcess
                .Get()
                .ToList();
        }
    }
}