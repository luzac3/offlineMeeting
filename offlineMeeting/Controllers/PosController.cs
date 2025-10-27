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
using System.Security.Cryptography;

namespace offlineMeeting.Controllers
{
    public class PosController : Controller
    {
        private readonly IHubContext<MyHub> _hubContext;
        private readonly ILogger<PosController> _logger;
        private PosViewModel PosViewModel;
        private int SakeSiteId;
        private int ExtraSiteId;
        private int EventFoodSiteId;
        private int RawFoodSiteId;
        private int OrderSiteId;
        private int OrderPurchaseSiteId;
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
            ExtraSiteId = tableIdProperty.ExtraSiteId;
            EventFoodSiteId = tableIdProperty.EventFoodSiteId;
            RawFoodSiteId = tableIdProperty.RawFoodSiteId;
            OrderSiteId = tableIdProperty.OrderSiteId;
            OrderPurchaseSiteId = tableIdProperty.OrderPurchaseSiteId;
            JoinerSiteId = tableIdProperty.JoinerSiteId;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            SetViewDataProcess setViewDataProcess = new();
            ViewData["UserData"] = setViewDataProcess.SetUserData(HttpContext);
            ViewData["Electron"] = setViewDataProcess.SetElectron();
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
            SiteSettingsProcess siteSettingsProcess = new ();

            PosViewModel.setAlcoholDataList(alcoholDataProcess.Get(siteSettingsProcess.Get(siteId: SakeSiteId), HttpContext, OrderSiteId));
            return View(PosViewModel);
        }

        public IActionResult Extra()
        {
            PurchaseDataProcess purchaseDataProcess = new(
                extraSiteId: ExtraSiteId,
                eventFoodSiteId: EventFoodSiteId,
                rawFoorSiteId: RawFoodSiteId,
                httpContext: HttpContext
            );
            SiteSettingsProcess siteSettingsProcess = new();

            PosViewModel.setPurchaseDataList(purchaseDataProcess.Get(HttpContext, OrderSiteId, 0));
            return View("Purchase", PosViewModel);
        }

        public IActionResult Food()
        {
            PurchaseDataProcess purchaseDataProcess = new(
                extraSiteId: ExtraSiteId,
                eventFoodSiteId: EventFoodSiteId,
                rawFoorSiteId: RawFoodSiteId,
                httpContext: HttpContext
            );
            SiteSettingsProcess siteSettingsProcess = new();

            PosViewModel.setPurchaseDataList(purchaseDataProcess.Get(HttpContext, OrderSiteId, 1));
            return View("Purchase", PosViewModel);
        }

        public IActionResult OrderList()
        {
            return View();
        }

        [HttpPost]
        public IActionResult OrderListContents([FromBody] List<OrderPostEntity> orderPostEntityList)
        {
            OrderListProcess orderListProcess = new (
                orderPostEntityList: orderPostEntityList,
                sakeSiteId: SakeSiteId,
                extraFoodSiteId: ExtraSiteId,
                eventFoodSiteId: EventFoodSiteId,
                rawFoodSiteId: RawFoodSiteId,
                httpContext: HttpContext
            );
            SiteSettingsProcess siteSettingsProcess = new ();

            PosViewModel.setOrderEntityList(orderListProcess.Get(siteSettingsProcess.Get(siteId: SakeSiteId), HttpContext, OrderSiteId));
            return PartialView("_OrderListContents", PosViewModel);
        }

        [HttpPost]
        public async Task<string> SendOrder([FromBody] List<OrderPostEntity> orderPostEntityList)
        {
            SendOrderProcess sendOrderProcess = new(
                orderPostEntityList,
                orderSiteId: OrderSiteId,
                orderPurchseSiteId: OrderPurchaseSiteId,
                joinerSiteId: JoinerSiteId,
                httpContext: HttpContext,
                hubContext: _hubContext
            );
            return await sendOrderProcess.Send(SakeSiteId);
        }

        [HttpGet]
        public IActionResult UserOrders([FromQuery] string eid)
        {
            SetViewDataProcess setViewDataProcess = new();
            // electron経由で表示する場合
            if (!string.IsNullOrEmpty(eid))
            {
                ViewData["Electron"] = setViewDataProcess.SetElectron(true);
            }

            GetUsersOrderProcess getUsersOrderProcess = new(
                OrderSiteId,
                OrderPurchaseSiteId,
                JoinerSiteId,
                HttpContext,
                eid
            );
            PosViewModel.setOrderTableEntityList(getUsersOrderProcess.Get());

            return View(PosViewModel);
        }

        [HttpGet]
        public IActionResult UserOrdersPartial([FromQuery] string eid)
        {
            GetUsersOrderProcess getUsersOrderProcess = new(
                OrderSiteId,
                OrderPurchaseSiteId,
                JoinerSiteId,
                HttpContext,
                eid
            );
            PosViewModel.setOrderTableEntityList(getUsersOrderProcess.Get());

            return PartialView("_UserOrderContents", PosViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> OrderComplete([FromBody] OrderCompletePostEntity orderCompletePostEntity)
        {
            OrderCompleteProcess orderCompleteProcess = new(orderCompletePostEntity.ResultIdList);

            return Json(await orderCompleteProcess.Send());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult OrderHistory()
        {
            GetUsersOrderProcess getUsersOrderProcess = new(
                OrderSiteId,
                OrderPurchaseSiteId,
                JoinerSiteId,
                HttpContext
            );
            PosViewModel.setOrderTableEntityList(getUsersOrderProcess.GetOwn(HttpContext));

            return View(PosViewModel);
        }

        [HttpGet]
        public IActionResult PrintOrders([FromQuery] string eid, string resultId)
        {
            GetUsersOrderProcess getUsersOrderProcess = new(
                OrderSiteId,
                OrderPurchaseSiteId,
                JoinerSiteId,
                HttpContext,
                eid
            );
            PosViewModel.setOrderTableEntityList(getUsersOrderProcess.Get(resultId));

            return View(PosViewModel);
        }
    }
}
