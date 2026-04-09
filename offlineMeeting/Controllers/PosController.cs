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
using System.Text.Json;

namespace offlineMeeting.Controllers
{
    public class PosController : Controller
    {
        private readonly IHubContext<MyHub> _hubContext;
        private readonly ILogger<PosController> _logger;
        private PosViewModel PosViewModel;
        private ResponseLogProcess ResponseLogProcess;
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
            ResponseLogProcess = new (_logger);
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
            try
            {
                AlcoholDataProcess alcoholDataProcess = new (sakeSiteId: SakeSiteId, httpContext: HttpContext);
                SiteSettingsProcess siteSettingsProcess = new ();

                PosViewModel.setAlcoholDataList(alcoholDataProcess.Get(siteSettingsProcess.Get(siteId: SakeSiteId), HttpContext, OrderSiteId));
                return View(PosViewModel);
            }
            catch (Exception ex)
            {
                CreateLogProcess.Error("POS_001", ex);
                throw;
            }
        }

        public IActionResult Extra()
        {
            try
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
            catch (Exception ex)
            {
                CreateLogProcess.Error("POS_002", ex);
                throw;
            }
        }

        public IActionResult Food()
        {
            try
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
            catch (Exception ex)
            {
                CreateLogProcess.Error("POS_003", ex);
                throw;
            }
        }

        public IActionResult OrderList()
        {
            return View();
        }

        [HttpPost]
        public IActionResult OrderListContents([FromBody] List<OrderPostEntity> orderPostEntityList)
        {
            try
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
            catch (Exception ex)
            {
                CreateLogProcess.Error("POS_004", ex);
                throw;
            }
        }

        [HttpPost]
        public string SendOrder([FromBody] List<OrderPostEntity> orderPostEntityList)
        {
            try
            {
                SendOrderProcess sendOrderProcess = new(
                    orderPostEntityList,
                    orderSiteId: OrderSiteId,
                    orderPurchseSiteId: OrderPurchaseSiteId,
                    joinerSiteId: JoinerSiteId,
                    httpContext: HttpContext,
                    hubContext: _hubContext
                );

                _ = Task.Run(async () =>
                {
                    try
                    {
                        string result = await sendOrderProcess.Send(SakeSiteId);

                        var dict = JsonSerializer.Deserialize<Dictionary<string, string>>(result);
                        string status = dict?["status"] ?? "";

                        ResponseLogProcess.OutputResponseLog(status, result ?? "");
                    }
                    catch (Exception ex)
                    {
                        CreateLogProcess.Error("POS_005", ex);
                    }
                });

                return JsonSerializer.Serialize(new Dictionary<string, string> { { "status", "200" }, { "message", "送信完了" } });
            }
            catch (Exception ex)
            {
                CreateLogProcess.Error("POS_005", ex);
                return JsonSerializer.Serialize(new Dictionary<string, string> { { "status", "500" }, { "message", ex.Message } });
            }
        }

        [HttpGet]
        public IActionResult UserOrders([FromQuery] string eid)
        {
            bool isElectron = !string.IsNullOrEmpty(eid);
            try
            {
                SetViewDataProcess setViewDataProcess = new();
                // electron経由で表示する場合
                if (isElectron)
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
            catch (Exception ex)
            {
                CreateLogProcess.Error("POS_006", ex, isElectron);
                throw;
            }
        }

        [HttpGet]
        public IActionResult UserOrdersPartial([FromQuery] string eid)
        {
            bool isElectron = !string.IsNullOrEmpty(eid);
            try
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
            catch (Exception ex)
            {
                CreateLogProcess.Error("POS_007", ex, isElectron);
                throw;
            }
        }

        [HttpPost]
        public string OrderComplete([FromBody] OrderCompletePostEntity orderCompletePostEntity)
        {
            try
            {
                OrderCompleteProcess orderCompleteProcess = new(orderCompletePostEntity.ResultIdList);

                _ = Task.Run(async () =>
                {
                    try
                    {
                        List<Dictionary<string, string>> results = await orderCompleteProcess.Send();

                        ResponseLogProcess.OrderCompleteLog(results);
                    }
                    catch (Exception ex)
                    {
                        CreateLogProcess.Error("POS_008", ex);
                    }
                });

                return JsonSerializer.Serialize(new Dictionary<string, string> { { "status", "200" } });
            }
            catch (Exception ex)
            {
                CreateLogProcess.Error("POS_008", ex);
                return JsonSerializer.Serialize(new Dictionary<string, string> { { "status", "500" }, { "message", ex.Message } });
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult OrderHistory()
        {
            try
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
            catch (Exception ex)
            {
                CreateLogProcess.Error("POS_009", ex);
                throw;
            }
        }

        [HttpGet]
        public IActionResult PrintOrders([FromQuery] string eid, string resultId)
        {
            bool isElectron = !string.IsNullOrEmpty(eid);
            try
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
            catch (Exception ex)
            {
                CreateLogProcess.Error("POS_010", ex, isElectron);
                throw;
            }
        }
    }
}
