using offlineMeeting.Models.Entity.Pos;
using offlineMeeting.Models.Share;
using System.Data;

namespace offlineMeeting.Models.Process.Pos
{
    public class FoodDataProcess
    {
        private int EventFoodSiteId;
        private string EventId;

        public FoodDataProcess(int eventFoodSiteId, HttpContext httpContext)
        {
            ManageEventId manageEventId = new (httpContext);

            EventFoodSiteId = eventFoodSiteId;
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

            SetFoodDataEntityListProcess setFoodDataEntityListProcess = new (EventFoodSiteId, orderSiteId, EventId, userCd);
            return setFoodDataEntityListProcess
                .Get()
                .ToList();
        }
    }
}