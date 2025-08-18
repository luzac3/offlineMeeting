using Microsoft.AspNetCore.Mvc;
using offlineMeeting.Models;
using offlineMeeting.Models.Share;
using System.Diagnostics;

namespace offlineMeeting.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }


        [HttpGet]
        public IActionResult Index([FromQuery] string uid, string eid)
        {
            ManageUserCd manageUserCd = new ManageUserCd(HttpContext);
            ManageEventId manageEventId = new ManageEventId(HttpContext);
            manageUserCd.SetUserCd(uid);
            manageEventId.SetEventId(eid);

            return View();
        }

        public IActionResult Privacy()
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