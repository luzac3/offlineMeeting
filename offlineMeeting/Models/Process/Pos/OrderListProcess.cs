using offlineMeeting.Models.Entity.Pos;
using offlineMeeting.Models.Entity.Share;
using offlineMeeting.Models.Process.Share;
using offlineMeeting.Models.Share;
using System.Data;

namespace offlineMeeting.Models.Process.Pos
{
    public class OrderListProcess
    {
        private int ExtraFoodSiteId;
        private int SakeSiteId;
        private int EventFoodSiteId;
        private int RawFoodSiteId;
        private string EventId;
        private List<OrderPostEntity> OrderPostEntityList;

        public OrderListProcess(
            List<OrderPostEntity> orderPostEntityList,
            int extraFoodSiteId,
            int sakeSiteId,
            int eventFoodSiteId,
            int rawFoodSiteId,
            HttpContext httpContext
        )
        {
            ManageEventId manageEventId = new (httpContext);

            EventId = manageEventId.GetEventId();
            if (EventId == "0")
            {
                CreateLogProcess.Error("POS_015", "イベントIDが不正です");
                throw new DataException("イベントIDが不正です");
            }
            ExtraFoodSiteId = extraFoodSiteId;
            SakeSiteId = sakeSiteId;
            EventFoodSiteId = eventFoodSiteId;
            RawFoodSiteId= rawFoodSiteId;
            OrderPostEntityList = orderPostEntityList.OrderBy(x => x.ResultId).ToList();
        }

        public List<OrderEntity> Get(System.Text.Json.Nodes.JsonNode? json, HttpContext httpContext, int orderSiteId)
        {
            List<OrderEntity> orderEntityList = new List<OrderEntity>();

            List<int> purchaseResultIdList = OrderPostEntityList
                .Where(x => x.Category != 2)
                .Select(x => x.ResultId)
                .ToList();

            List<int> alcoholResultIdList = OrderPostEntityList
                .Where(x => x.Category == 2)
                .Select(x => x.ResultId)
                .ToList();

            Dictionary<int, int> categoryDic = OrderPostEntityList
                .ToDictionary(x => x.ResultId, x => x.Category);

            ManageUserCd manageUserCd = new (httpContext);
            string userCd = manageUserCd.GetUserCd().ToString();

            SetPurchaseDataEntityListProcess setPurchaseDataEntityListProcess = new(
                ExtraFoodSiteId,
                EventFoodSiteId,
                RawFoodSiteId,
                orderSiteId,
                EventId,
                userCd
            );
            List<PurchaseDataEntity> purchaseDataEntityList = setPurchaseDataEntityListProcess.Get(purchaseResultIdList, categoryDic);

            SetAlcoholDataEntityProcess setAlcoholDataEntityListProcess = new(SakeSiteId, orderSiteId, EventId, userCd);
            List<AlcoholDataEntity> alcoholDataEntityList = setAlcoholDataEntityListProcess.Get(json, alcoholResultIdList);

            foreach (OrderPostEntity orderPostEntity in OrderPostEntityList)
            {
                orderEntityList.Add(
                    new OrderEntity
                    (
                        orderPostEntity: orderPostEntity,
                        purchaseDataEntity: purchaseDataEntityList.Where(x => x.ResultId == orderPostEntity.ResultId).FirstOrDefault(),
                        alcoholDataEntity: alcoholDataEntityList.Where(x => x.ResultId == orderPostEntity.ResultId).FirstOrDefault()
                    )
                );
            }
            return orderEntityList;
        }
    }
}