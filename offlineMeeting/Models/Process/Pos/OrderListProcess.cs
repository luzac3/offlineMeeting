using offlineMeeting.Models.Entity.Pos;
using offlineMeeting.Models.Entity.Share;
using offlineMeeting.Models.Process.Share;
using offlineMeeting.Models.Share;
using PleasanterBridge.src.DataRepository.Service;
using System.Data;

namespace offlineMeeting.Models.Process.Pos
{
    public class OrderListProcess
    {
        private int ExtraFoodSiteId;
        private int SakeSiteId;
        private int EventFoodSiteId;
        private int RawFoodSiteId;
        private int EventId;
        private List<OrderPostEntity> OrderPostEntityList;
        private readonly IPleasanterRepository _pleasanterRepository;

        public OrderListProcess(
            List<OrderPostEntity> orderPostEntityList,
            int extraFoodSiteId,
            int sakeSiteId,
            int eventFoodSiteId,
            int rawFoodSiteId,
            HttpContext httpContext,
            IPleasanterRepository pleasanterRepository
        )
        {
            ManageEventId manageEventId = new (httpContext);

            EventId = manageEventId.GetEventId();

            ExtraFoodSiteId = extraFoodSiteId;
            SakeSiteId = sakeSiteId;
            EventFoodSiteId = eventFoodSiteId;
            RawFoodSiteId= rawFoodSiteId;
            OrderPostEntityList = orderPostEntityList.OrderBy(x => x.ResultId).ToList();
            _pleasanterRepository = pleasanterRepository;
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

            Dictionary<string, int> categoryDic = OrderPostEntityList
                .ToDictionary(x => $"{x.ResultId}_{x.AlcoholAmount}", x => x.Category);

            ManageUserCd manageUserCd = new (httpContext);
            int userCd = manageUserCd.GetUserCd();

            SetPurchaseDataEntityListProcess setPurchaseDataEntityListProcess = new(
                ExtraFoodSiteId,
                EventFoodSiteId,
                RawFoodSiteId,
                orderSiteId,
                EventId,
                userCd,
                _pleasanterRepository
            );
            List<PurchaseDataEntity> purchaseDataEntityList = setPurchaseDataEntityListProcess.Get(purchaseResultIdList, categoryDic);

            SetAlcoholDataEntityProcess setAlcoholDataEntityListProcess = new(_pleasanterRepository, SakeSiteId, orderSiteId, EventId, userCd);
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