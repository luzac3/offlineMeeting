using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.SignalR;
using offlineMeeting.Models;
using offlineMeeting.Models.Entity.Login;
using offlineMeeting.Models.JsonDataProperty;
using offlineMeeting.Models.Process.Collection;
using offlineMeeting.Models.Process.Share;
using offlineMeeting.Models.ViewModel;
using PleasanterBridge.src.DataRepository.Service;
using System.Diagnostics;

namespace offlineMeeting.Controllers
{
    public class CollectionController : Controller
    {
        private readonly IHubContext<MyHub> _hubContext;
        private readonly ILogger<CollectionController> _logger;
        private readonly IPleasanterRepository _pleasanterRepository;
        private CollectionViewModel CollectionViewModel;
        private int SakeSiteId;
        private int OrderSiteId;

        public CollectionController(
            ILogger<CollectionController> logger,
            IHubContext<MyHub> hubContext,
            IPleasanterRepository pleasanterRepository
        )
        {
            _logger = logger;
            _hubContext = hubContext;
            _pleasanterRepository = pleasanterRepository;
            CollectionViewModel = new ();
            TableIdProperty tableIdProperty = new ();
            SakeSiteId = tableIdProperty.SakeSiteId;
            OrderSiteId = tableIdProperty.OrderSiteId;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            SetViewDataProcess setViewDataProcess = new();
            ViewData["UserData"] = setViewDataProcess.SetUserData(HttpContext, _pleasanterRepository);
            base.OnActionExecuting(context);
        }

        public IActionResult Index()
        {
            AlcoholDataProcess alcoholDataProcess = new(
                httpContext: HttpContext,
                pleasanterRepository: _pleasanterRepository,
                sakeSiteId: SakeSiteId,
                orderSiteId: OrderSiteId
            );
            SiteSettingsProcess siteSettingsProcess = new();

            CollectionViewModel.setAlcoholDataList(alcoholDataProcess.Get(siteSettingsProcess.Get(siteId: SakeSiteId)));
            return View(CollectionViewModel);
        }

        [HttpGet("Collection/AlcoholDetail/{alcoholId}")]
        public IActionResult AlcoholDetail(int alcoholId)
        {
            AlcoholDataProcess alcoholDataProcess = new(
                httpContext: HttpContext, 
                pleasanterRepository: _pleasanterRepository,
                sakeSiteId: SakeSiteId, 
                orderSiteId: OrderSiteId
            );
            SiteSettingsProcess siteSettingsProcess = new();

            CollectionViewModel.setAlcoholDataEntity(alcoholDataProcess.Get(siteSettingsProcess.Get(siteId: SakeSiteId), alcoholId));
            return View(CollectionViewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
