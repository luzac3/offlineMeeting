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
using PleasanterBridge.src.DataRepository.Service;
using System.Diagnostics;
using System.Text.Json;

namespace offlineMeeting.Controllers
{
    public class PosController : Controller
    {
        private readonly IHubContext<MyHub> _hubContext;
        private readonly ILogger<PosController> _logger;
        private readonly IWebHostEnvironment _env;
        private readonly IPleasanterRepository _pleasanterRepository;
        private PosViewModel PosViewModel;
        private ResponseLogProcess ResponseLogProcess;
        private long SakeSiteId;
        private long ExtraSiteId;
        private long EventFoodSiteId;
        private long RawFoodSiteId;
        private long OrderSiteId;
        private long OrderPurchaseSiteId;
        private long JoinerSiteId;

        public PosController(
            ILogger<PosController> logger,
            IHubContext<MyHub> hubContext,
            IWebHostEnvironment env,
            IPleasanterRepository pleasanterRepository
        )
        {
            _logger = logger;
            _hubContext = hubContext;
            _env = env;
            _pleasanterRepository = pleasanterRepository;
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
            ViewData["UserData"] = setViewDataProcess.SetUserData(HttpContext, _pleasanterRepository);
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

            PosViewModel.setAlcoholDataList(alcoholDataProcess.Get(siteSettingsProcess.Get(siteId: SakeSiteId), HttpContext, _pleasanterRepository, OrderSiteId));
            return View(PosViewModel);
        }

        public IActionResult Extra()
        {
            PurchaseDataProcess purchaseDataProcess = new(
                extraSiteId: ExtraSiteId,
                eventFoodSiteId: EventFoodSiteId,
                rawFoorSiteId: RawFoodSiteId,
                httpContext: HttpContext,
                pleasanterRepository: _pleasanterRepository
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
                httpContext: HttpContext,
                pleasanterRepository: _pleasanterRepository
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
                httpContext: HttpContext,
                pleasanterRepository: _pleasanterRepository
            );
            SiteSettingsProcess siteSettingsProcess = new ();

            PosViewModel.setOrderEntityList(orderListProcess.Get(siteSettingsProcess.Get(siteId: SakeSiteId), HttpContext, OrderSiteId));
            return PartialView("_OrderListContents", PosViewModel);
        }

        [HttpPost]
        public string SendOrder([FromBody] List<OrderPostEntity> orderPostEntityList)
        {
            SendOrderProcess sendOrderProcess = new(
                orderPostEntityList,
                orderSiteId: OrderSiteId,
                orderPurchseSiteId: OrderPurchaseSiteId,
                joinerSiteId: JoinerSiteId,
                httpContext: HttpContext,
                hubContext: _hubContext,
                pleasanterRepository: _pleasanterRepository
            );

            _ = Task.Run(async () =>
            {
                string result = await sendOrderProcess.Send(SakeSiteId);

                var dict = JsonSerializer.Deserialize<Dictionary<string, string>>(result);
                string status = dict?["status"] ?? "";

                ResponseLogProcess.OutputResponseLog(status, result ?? "");
            });

            return JsonSerializer.Serialize(new Dictionary<string, string> { { "status", "200" }, { "message", "送信完了" } });
        }

        [HttpGet]
        public IActionResult UserOrders([FromQuery] long eid)
        {
            SetViewDataProcess setViewDataProcess = new();
            // electron経由で表示する場合
            if (eid != 0)
            {
                ViewData["Electron"] = setViewDataProcess.SetElectron(true);
            }

            GetUsersOrderProcess getUsersOrderProcess = new(
                OrderSiteId,
                OrderPurchaseSiteId,
                JoinerSiteId,
                HttpContext,
                _pleasanterRepository,
                eid
            );
            PosViewModel.setOrderTableEntityList(getUsersOrderProcess.Get());

            return View(PosViewModel);
        }

        [HttpGet]
        public IActionResult UserOrdersPartial([FromQuery] long eid)
        {
            GetUsersOrderProcess getUsersOrderProcess = new(
                OrderSiteId,
                OrderPurchaseSiteId,
                JoinerSiteId,
                HttpContext,
                _pleasanterRepository,
                eid
            );
            PosViewModel.setOrderTableEntityList(getUsersOrderProcess.Get());

            return PartialView("_UserOrderContents", PosViewModel);
        }

        [HttpPost]
        public string OrderComplete([FromBody] OrderCompletePostEntity orderCompletePostEntity)
        {
            OrderCompleteProcess orderCompleteProcess = new(orderCompletePostEntity.ResultIdList);

            _ = Task.Run(async () =>
            {
                List<Dictionary<string, string>> results = await orderCompleteProcess.Send();

                ResponseLogProcess.OrderCompleteLog(results);
            });

            return JsonSerializer.Serialize(new Dictionary<string, string> { { "status", "200" } });
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
                HttpContext,
                _pleasanterRepository
            );
            PosViewModel.setOrderTableEntityList(getUsersOrderProcess.GetOwn(HttpContext));

            return View(PosViewModel);
        }

        [HttpGet]
        public IActionResult PrintOrders([FromQuery] long eid, long resultId)
        {
            GetUsersOrderProcess getUsersOrderProcess = new(
                OrderSiteId,
                OrderPurchaseSiteId,
                JoinerSiteId,
                HttpContext,
                _pleasanterRepository,
                eid
            );
            PosViewModel.setOrderTableEntityList(getUsersOrderProcess.Get(resultId));

            return View(PosViewModel);
        }

        [HttpGet]
        public IActionResult PrintQrOrders([FromQuery] long eid, long resultId)
        {
            GetUsersOrderProcess getUsersOrderProcess = new(
                OrderSiteId,
                OrderPurchaseSiteId,
                JoinerSiteId,
                HttpContext,
                _pleasanterRepository,
                eid
            );
            PosViewModel.setOrderTableEntityList(getUsersOrderProcess.Get(resultId));
            PosViewModel.setPrintUrl($"https://sake.wolfslab.jp/Pos/PrintOrders?eid={eid}&resultId={resultId}");

            return View(PosViewModel);
        }

        [HttpGet]
        public IActionResult NoImageRender([FromQuery] string src)
        {
            using ImageHelper imageHelper = new ImageHelper(System.IO.File.ReadAllBytes(
                    PathHelper.WebPathToPhysicalPath(_env, src)
                )
            );

            byte[]? bytes = imageHelper.GetLowQualityBytes();
            return File(bytes, "image/webp");
        }
    }
}