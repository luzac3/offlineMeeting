using Microsoft.AspNetCore.Mvc;
using offlineMeeting.Models;
using offlineMeeting.Models.Entity.Coins;
using offlineMeeting.Models.JsonDataProperty;
using offlineMeeting.Models.Process.Coins;
using offlineMeeting.Models.Process.DBProcess;
using offlineMeeting.Models.Process.Share;
using offlineMeeting.Models.ViewModel;
using System.Diagnostics;

namespace offlineMeeting.Controllers
{
    public class CoinsController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private int CoinsSiteId;
        private int JoinerSiteId;

        public CoinsController(ILogger<HomeController> logger)
        {
            _logger = logger;
            TableIdProperty tableIdProperty = new();
            CoinsSiteId = tableIdProperty.CoinsSiteId;
            JoinerSiteId = tableIdProperty.JoinerSiteId;
        }

        public IActionResult Index()
        {
            CoinsViewModel coinsViewModel = new();
            return View(coinsViewModel);
        }

        public IActionResult Buy()
        {
            CoinsViewModel coinsViewModel = new();
            return View(coinsViewModel);
        }

        public IActionResult Give()
        {
            GetUsersProcess getUsersProcess = new(JoinerSiteId, HttpContext);
            GiveCoinsViewModel giveCoinsViewModel = new(getUsersProcess.GetUsers());
            return View(giveCoinsViewModel);
        }
        
        [HttpPost]
        public async Task<IActionResult> PaymentComplete([FromBody] CoinsEntity coinsEntity)
        {
            SendCoinsProcess sendCoinsProcess = new (coinsEntity: coinsEntity, coinsSiteId: CoinsSiteId, httpContext: HttpContext);

            bool result = await sendCoinsProcess.Send();
            return Json(result.ToString());
        }

        [HttpPost]
        public async Task<IActionResult> GiveCoins([FromBody] GiveCoinsEntity giveCoinsEntity)
        {
            GiveCoinsProcess giveCoinsProcess = new (giveCoinsEntity: giveCoinsEntity, coinsSiteId: CoinsSiteId, httpContext: HttpContext);

            try
            {
                bool result = await giveCoinsProcess.Send();

                return Json(result.ToString());
            }
            catch (Exception ex)
            {
                return Json(ex.ToString());
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}