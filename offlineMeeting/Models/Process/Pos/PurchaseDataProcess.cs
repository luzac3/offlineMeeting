using offlineMeeting.Models.Entity.Pos;
using offlineMeeting.Models.Share;
using PleasanterBridge.src.DataRepository.Service;

namespace offlineMeeting.Models.Process.Pos
{
    public class PurchaseDataProcess
    {
        private long ExtraSiteId;
        private long EventFoodSiteId;
        private long RawFoorSiteId;
        private long EventId;
        private readonly IPleasanterRepository _pleasanterRepository;

        public PurchaseDataProcess(
            long extraSiteId,
            long eventFoodSiteId,
            long rawFoorSiteId,
            HttpContext httpContext,
            IPleasanterRepository pleasanterRepository
        )
        {
            ManageEventId manageEventId = new (httpContext);
            _pleasanterRepository = pleasanterRepository;

            ExtraSiteId = extraSiteId;
            EventFoodSiteId = eventFoodSiteId;
            RawFoorSiteId = rawFoorSiteId;
            EventId = manageEventId.GetEventId();
        }

        public List<PurchaseDataEntity> Get(HttpContext httpContext, long orderSiteId, int category)
        {
            ManageUserCd manageUserCd = new ManageUserCd(httpContext);
            long userCd = manageUserCd.GetUserCd();

            SetPurchaseDataEntityListProcess setPurchaseDataEntityListProcess = new (
                ExtraSiteId,
                EventFoodSiteId,
                RawFoorSiteId,
                orderSiteId,
                EventId,
                userCd,
                _pleasanterRepository
            );
            return setPurchaseDataEntityListProcess
                .Get(category)
                .ToList();
        }
    }
}