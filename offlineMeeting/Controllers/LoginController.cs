using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using offlineMeeting.Models;
using offlineMeeting.Models.Entity.Login;
using offlineMeeting.Models.Process.Home;
using offlineMeeting.Models.Process.Login;
using offlineMeeting.Models.Share;
using offlineMeeting.Models.ViewModel;
using System.Diagnostics;

namespace offlineMeeting.Controllers
{
    public class LoginController : Controller
    {
        private readonly IHubContext<MyHub> _hubContext;
        private readonly ILogger<HomeController> _logger;

        public LoginController(
            ILogger<HomeController> logger,
            IHubContext<MyHub> hubContext
        )
        {
            _logger = logger;
            _hubContext = hubContext;
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
                return Redirect($"~/Home/Index");
            }
            else
            {
                return Redirect($"~/Login/Qr?eid={eid}&uid={uid}");
            }
        }

        [HttpGet]
        public IActionResult Qr([FromQuery] string uid, string eid)
        {
            GenerateQr generateQr = new (
                eventId: Int32.TryParse(eid, out int parsedEid) ? parsedEid : 0,
                userId: Int32.TryParse(uid, out int parsedUid) ? parsedUid : 0
            );
            LoginViewModel loginViewModel = new ();
            loginViewModel.GenerateQr = generateQr;
            return View(loginViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> QrRegister([FromQuery] string eid, string uid)
        {
            // 特定日時の設定
            var openDate = new DateTime(2025, 9, 7, 10, 0, 0);

            // 現在時刻がopenDateより前ならリダイレクト
            if (DateTime.Now < openDate)
            {
                return Redirect("~/Login/ComingSoon");
            }

            SendRegisterProcess sendRegisterProcess = new (hubContext: _hubContext, eventId: eid, userCd: uid);
            RegisterViewModel registerViewModel = new (await sendRegisterProcess.Send());

            return View(registerViewModel);
        }

        public IActionResult ComingSoon()
        {
            return View();
        }
    }
}