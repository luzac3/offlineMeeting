using Microsoft.AspNetCore.Mvc;
using offlineMeeting.Models;
using offlineMeeting.Models.Entity.Coins;
using offlineMeeting.Models.JsonDataProperty;
using offlineMeeting.Models.Process.Coins;
using offlineMeeting.Models.ViewModel;
using System.Diagnostics;

namespace offlineMeeting.Controllers
{
    public class CoinsController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private int CoinsSiteId;

        public CoinsController(ILogger<HomeController> logger)
        {
            _logger = logger;
            TableIdProperty tableIdProperty = new();
            CoinsSiteId = tableIdProperty.CoinsSiteId;
        }

        public IActionResult Index()
        {
            CoinsViewModel coinsViewModel = new();
            return View(coinsViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> PaymentComplete([FromBody] CoinsEntity coinsEntity)
        {
            SendCoinsProcess sendCoinsProcess = new (coinsEntity: coinsEntity, coinsSiteId: CoinsSiteId, httpContext: HttpContext);

            bool result = await sendCoinsProcess.Send();
            return Json(result.ToString());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}