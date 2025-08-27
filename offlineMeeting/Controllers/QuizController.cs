using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using offlineMeeting.Models;
using offlineMeeting.Models.Entity.Coins;
using offlineMeeting.Models.JsonDataProperty;
using offlineMeeting.Models.Process.Coins;
using offlineMeeting.Models.Process.Share;
using offlineMeeting.Models.ViewModel;
using System.Diagnostics;

namespace offlineMeeting.Controllers
{
    public class QuizController : Controller
    {
        private readonly ILogger<QuizController> _logger;
        private int CoinsSiteId;
        private int JoinerSiteId;

        public QuizController(ILogger<QuizController> logger)
        {
            _logger = logger;
            TableIdProperty tableIdProperty = new();
            CoinsSiteId = tableIdProperty.CoinsSiteId;
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

        public IActionResult Orner()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}