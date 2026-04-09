using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.SignalR;
using offlineMeeting.Models;
using offlineMeeting.Models.Entity.Login;
using offlineMeeting.Models.JsonDataProperty;
using offlineMeeting.Models.Process.Collection;
using offlineMeeting.Models.Process.Share;
using offlineMeeting.Models.ViewModel;
using System.Diagnostics;

namespace offlineMeeting.Controllers
{
    public class CollectionController : Controller
    {
        private readonly IHubContext<MyHub> _hubContext;
        private readonly ILogger<CollectionController> _logger;
        private CollectionViewModel CollectionViewModel;
        private int SakeSiteId;
        private int OrderSiteId;

        public CollectionController(
            ILogger<CollectionController> logger,
            IHubContext<MyHub> hubContext
        )
        {
            _logger = logger;
            _hubContext = hubContext;
            CollectionViewModel = new ();
            TableIdProperty tableIdProperty = new ();
            SakeSiteId = tableIdProperty.SakeSiteId;
            OrderSiteId = tableIdProperty.OrderSiteId;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            SetViewDataProcess setViewDataProcess = new();
            ViewData["UserData"] = setViewDataProcess.SetUserData(HttpContext);
            base.OnActionExecuting(context);
        }

        public IActionResult Index()
        {
            try
            {
                AlcoholDataProcess alcoholDataProcess = new(sakeSiteId: SakeSiteId, orderSiteId: OrderSiteId, httpContext: HttpContext);
                SiteSettingsProcess siteSettingsProcess = new();

                CollectionViewModel.setAlcoholDataList(alcoholDataProcess.Get(siteSettingsProcess.Get(siteId: SakeSiteId)));
                return View(CollectionViewModel);
            }
            catch (Exception ex)
            {
                CreateLogProcess.Error("COLLECTION_001", ex);
                throw;
            }
        }

        [HttpGet("Collection/AlcoholDetail/{alcoholId}")]
        public IActionResult AlcoholDetail(int alcoholId)
        {
            try
            {
                AlcoholDataProcess alcoholDataProcess = new(sakeSiteId: SakeSiteId, orderSiteId: OrderSiteId, httpContext: HttpContext);
                SiteSettingsProcess siteSettingsProcess = new();

                CollectionViewModel.setAlcoholDataEntity(alcoholDataProcess.Get(siteSettingsProcess.Get(siteId: SakeSiteId), alcoholId));
                return View(CollectionViewModel);
            }
            catch (Exception ex)
            {
                CreateLogProcess.Error("COLLECTION_002", ex);
                throw;
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
