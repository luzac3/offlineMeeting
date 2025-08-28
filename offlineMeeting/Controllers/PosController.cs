using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.SignalR;
using offlineMeeting.Models;
using offlineMeeting.Models.Entity.Login;
using offlineMeeting.Models.Entity.Pos;
using offlineMeeting.Models.JsonDataProperty;
using offlineMeeting.Models.Process.Pos;
using offlineMeeting.Models.Process.Share;
using offlineMeeting.Models.ViewModel;
using System.Diagnostics;

namespace offlineMeeting.Controllers
{
    public class PosController : Controller
    {
        private readonly IHubContext<MyHub> _hubContext;
        private readonly ILogger<PosController> _logger;
        private PosViewModel PosViewModel;
        private int SakeSiteId;
        private int OrderSiteId;
        private int JoinerSiteId;

        public PosController(
            ILogger<PosController> logger,
            IHubContext<MyHub> hubContext
        )
        {
            _logger = logger;
            _hubContext = hubContext;
            PosViewModel = new ();
            TableIdProperty tableIdProperty = new ();
            SakeSiteId = tableIdProperty.SakeSiteId;
            OrderSiteId = tableIdProperty.OrderSiteId;
            JoinerSiteId = tableIdProperty.JoinerSiteId;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            SetViewDataProcess setViewDataProcess = new();
            ViewData["UserData"] = setViewDataProcess.Set(HttpContext);
            base.OnActionExecuting(context);
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Appetizers()
        {
            return View();
        }

        public IActionResult Alcohol()
        {
            AlcoholDataProcess alcoholDataProcess = new (sakeSiteId: SakeSiteId, httpContext: HttpContext);
            SiteSettingsProcess siteSettingsProcess = new (sakeSiteId: SakeSiteId);

            PosViewModel.setAlcoholDataList(alcoholDataProcess.Get(siteSettingsProcess.Get(), HttpContext, OrderSiteId));
            return View(PosViewModel);
        }

        public IActionResult OrderList()
        {
            return View();
        }

        [HttpPost]
        public IActionResult OrderListContents([FromBody] List<OrderPostEntity> orderPostEntityList)
        {
            OrderListProcess orderListProcess = new (orderPostEntityList: orderPostEntityList, sakeSiteId: SakeSiteId, httpContext: HttpContext);
            SiteSettingsProcess siteSettingsProcess = new (sakeSiteId: SakeSiteId);

            PosViewModel.setOrderEntityList(orderListProcess.Get(siteSettingsProcess.Get(), HttpContext, OrderSiteId));
            return PartialView("_OrderListContents", PosViewModel);
        }

        [HttpPost]
        public async Task<string> SendOrder([FromBody] List<OrderPostEntity> orderPostEntityList)
        {
            SendOrderProcess sendOrderProcess = new(
                orderPostEntityList,
                orderSiteId: OrderSiteId,
                joinerSiteId: JoinerSiteId,
                httpContext: HttpContext,
                hubContext: _hubContext
            );
            return await sendOrderProcess.Send();
        }

        public IActionResult UserOrders()
        {
            GetUsersOrderProcess getUsersOrderProcess = new (OrderSiteId, JoinerSiteId, HttpContext);
            PosViewModel.setOrderTableEntityList(getUsersOrderProcess.Get());

            return View(PosViewModel);
        }

        public IActionResult UserOrdersPartial()
        {
            GetUsersOrderProcess getUsersOrderProcess = new(OrderSiteId, JoinerSiteId, HttpContext);
            PosViewModel.setOrderTableEntityList(getUsersOrderProcess.Get());

            return PartialView("_UserOrderContents", PosViewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
