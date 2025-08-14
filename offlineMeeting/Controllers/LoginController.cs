using offlineMeeting.Models;
using offlineMeeting.Models.JsonDataProperty;
using offlineMeeting.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.ViewEngines;

namespace offlineMeeting.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public LoginController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index([FromQuery] string UserId)
        {
            return Json(UserId);
        }

        [HttpGet]
        public IActionResult QrRegister([FromQuery] string UserId)
        {
            return Json("register completed"); // 登録処理
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}