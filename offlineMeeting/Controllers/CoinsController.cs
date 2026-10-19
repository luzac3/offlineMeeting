using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using offlineMeeting.Models;
using offlineMeeting.Models.Entity.Coins;
using offlineMeeting.Models.Entity.Share;
using offlineMeeting.Models.JsonDataProperty;
using offlineMeeting.Models.Process.Coins;
using offlineMeeting.Models.Process.Share;
using offlineMeeting.Models.ViewModel;
using PleasanterBridge.src.APIBridge;
using PleasanterBridge.src.DataRepository.Service;
using System.Diagnostics;

namespace offlineMeeting.Controllers
{
    public class CoinsController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IPleasanterApiBridge _bridge;
        private readonly IPleasanterRepository _pleasanterRepository;
        CoinsViewModel CoinsViewModel;
        private long CoinsSiteId;
        private long JoinerSiteId;

        public CoinsController(ILogger<HomeController> logger, IPleasanterApiBridge bridge, IPleasanterRepository pleasanterRepository)
        {
            _logger = logger;
            _bridge = bridge;
            _pleasanterRepository = pleasanterRepository;
            CoinsViewModel = new();
            TableIdProperty tableIdProperty = new();
            CoinsSiteId = tableIdProperty.CoinsSiteId;
            JoinerSiteId = tableIdProperty.JoinerSiteId;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            SetViewDataProcess setViewDataProcess = new();
            ViewData["UserData"] = setViewDataProcess.SetUserData(HttpContext, _pleasanterRepository);
            base.OnActionExecuting(context);
        }

        public IActionResult Index()
        {
            return View(CoinsViewModel);
        }

        public IActionResult Buy()
        {
            return View(CoinsViewModel);
        }

        public IActionResult Give()
        {
            GetUsersProcess getUsersProcess = new(HttpContext, _pleasanterRepository, JoinerSiteId);
            GiveCoinsViewModel giveCoinsViewModel = new(getUsersProcess.GetUsers());
            return View(giveCoinsViewModel);
        }
        
        [HttpPost]
        public async Task<IActionResult> PaymentComplete([FromBody] CoinsEntity coinsEntity)
        {
            SendCoinsProcess sendCoinsProcess = new(
                httpContext: HttpContext,
                bridge: _bridge,
                coinsEntity: coinsEntity,
                coinsSiteId: CoinsSiteId
            );

            bool result = await sendCoinsProcess.Send();
            return Json(result.ToString());
        }

        [HttpPost]
        public async Task<IActionResult> GiveCoins([FromBody] GiveCoinsEntity giveCoinsEntity)
        {
            GiveCoinsProcess giveCoinsProcess = new (httpContext: HttpContext, bridge: _bridge, giveCoinsEntity: giveCoinsEntity, coinsSiteId: CoinsSiteId);

            Dictionary<string, string> result = await giveCoinsProcess.Send();

            return Json(result);
        }

        [HttpPost]
        public IActionResult TogglePaypalMode()
        {
            JoinerEntity? userData = ViewData["UserData"] as JoinerEntity;
            if (userData == null || !userData.IsAdmin)
            {
                return Forbid();
            }

            PaypalProperty.ToggleMode();
            string referer = Request.Headers.Referer.ToString();
            return Redirect(string.IsNullOrEmpty(referer) ? "/" : referer);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}