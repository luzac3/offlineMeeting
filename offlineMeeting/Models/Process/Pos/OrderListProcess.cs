using offlineMeeting.Models.Entity.Pos;
using offlineMeeting.Models.Entity.Share;
using offlineMeeting.Models.Process.Share;
using offlineMeeting.Models.Share;
using System.Data;

namespace offlineMeeting.Models.Process.Pos
{
    public class OrderListProcess
    {
        private int SakeSiteId;
        private int EventFoodSiteId;
        private string EventId;
        private List<OrderPostEntity> OrderPostEntityList;

        public OrderListProcess(List<OrderPostEntity> orderPostEntityList, int sakeSiteId, int eventFoodSiteId, HttpContext httpContext)
        {
            ManageEventId manageEventId = new (httpContext);

            EventId = manageEventId.GetEventId();
            if (EventId == "0")
            {
                throw new DataException("イベントIDが不正です");
            }
            SakeSiteId = sakeSiteId;
            EventFoodSiteId = eventFoodSiteId;
            OrderPostEntityList = orderPostEntityList.OrderBy(x => x.ResultId).ToList();
        }

        public List<OrderEntity> Get(System.Text.Json.Nodes.JsonNode? json, HttpContext httpContext, int orderSiteId)
        {
            List<OrderEntity> orderEntityList = new List<OrderEntity>();

            List<int> spResultIdList = OrderPostEntityList
                .Where(x => x.Category == 0)
                .Select(x => x.ResultId)
                .ToList();

            List<int> foodResultIdList = OrderPostEntityList
                .Where(x => x.Category == 1)
                .Select(x => x.ResultId)
                .ToList();

            List<int> alcoholResultIdList = OrderPostEntityList
                .Where(x => x.Category == 2)
                .Select(x => x.ResultId)
                .ToList();

            ManageUserCd manageUserCd = new (httpContext);
            string userCd = manageUserCd.GetUserCd().ToString();

            SetFoodDataEntityListProcess setFoodDataEntityListProcess = new(EventFoodSiteId, orderSiteId, EventId, userCd);
            List<FoodDataEntity> foodDataEntityList = setFoodDataEntityListProcess.Get(foodResultIdList);

            SetAlcoholDataEntityListProcess setAlcoholDataEntityListProcess = new(SakeSiteId, orderSiteId, EventId, userCd);
            List<AlcoholDataEntity> alcoholDataEntityList = setAlcoholDataEntityListProcess.Get(json, alcoholResultIdList);

            foreach (OrderPostEntity orderPostEntity in OrderPostEntityList)
            {
                orderEntityList.Add(
                    new OrderEntity
                    (
                        orderPostEntity: orderPostEntity,
                        foodDataEntity: foodDataEntityList.Where(x => x.ResultId == orderPostEntity.ResultId).FirstOrDefault(),
                        alcoholDataEntity: alcoholDataEntityList.Where(x => x.ResultId == orderPostEntity.ResultId).FirstOrDefault()
                    )
                );
            }

            return orderEntityList;
        }
    }
}