using Microsoft.AspNetCore.Mvc;
using offlineMeeting.Models;
using offlineMeeting.Models.Process.Home;
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
            ViewData["UserName"] = "るざ狼";
        }

        [HttpGet]
        public IActionResult Index([FromQuery] string uid, string eid)
        {
            ManageUserCd manageUserCd = new ManageUserCd(HttpContext);
            ManageEventId manageEventId = new ManageEventId(HttpContext);
            CheckUserJoin checkUserJoin = new CheckUserJoin(uid);
            manageUserCd.SetUserCd(uid);
            manageEventId.SetEventId(eid);

            // ユーザが登録状態であるかどうか
            if (checkUserJoin.Check())
            {
                // userを取得しに行く
                // なおuserCdはすべての場合に有効なのでHome以外ではコンストラクタで取得する。
                // Homeはただのメニュー兼エラー受け画面なので要らない

                ViewData["UserName"] = "るざ狼";
                return View();
            }
            else
            {
                return Redirect($"~/Login/Qr?eid={eid}&uid={uid}");
            }
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