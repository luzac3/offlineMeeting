using offlineMeeting.Models.Entity.Pos;
using offlineMeeting.Models.Share;
using PleasanterBridge.src.DataRepository.Service;

namespace offlineMeeting.Models.Process.Pos
{
    public class PurchaseDataProcess
    {
        private int ExtraSiteId;
        private int EventFoodSiteId;
        private int RawFoorSiteId;
        private int EventId;
        private readonly IPleasanterRepository _pleasanterRepository;

        public PurchaseDataProcess(
            int extraSiteId,
            int eventFoodSiteId,
            int rawFoorSiteId,
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

        public List<PurchaseDataEntity> Get(HttpContext httpContext, int orderSiteId, int category)
        {
            ManageUserCd manageUserCd = new ManageUserCd(httpContext);
            int userCd = manageUserCd.GetUserCd();

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