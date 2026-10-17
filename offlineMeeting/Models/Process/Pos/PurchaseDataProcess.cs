using Microsoft.AspNetCore.Hosting;
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
        private readonly IWebHostEnvironment? _env;

        public PurchaseDataProcess(
            long extraSiteId,
            long eventFoodSiteId,
            long rawFoorSiteId,
            HttpContext httpContext,
            IPleasanterRepository pleasanterRepository,
            IWebHostEnvironment? env = null
        )
        {
            ManageEventId manageEventId = new (httpContext);
            _pleasanterRepository = pleasanterRepository;

            ExtraSiteId = extraSiteId;
            EventFoodSiteId = eventFoodSiteId;
            RawFoorSiteId = rawFoorSiteId;
            EventId = manageEventId.GetEventId();
            _env = env;
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
                _pleasanterRepository,
                _env
            );
            return setPurchaseDataEntityListProcess
                .Get(category)
                .ToList();
        }
    }
}