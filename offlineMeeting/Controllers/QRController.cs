using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.SignalR;
using offlineMeeting.Models;
using offlineMeeting.Models.Entity.Login;
using offlineMeeting.Models.Entity.Share;
using offlineMeeting.Models.Entity.QR;
using offlineMeeting.Models.JsonDataProperty;
using offlineMeeting.Models.Process.QR;
using offlineMeeting.Models.Process.Share;
using offlineMeeting.Models.Share;
using offlineMeeting.Models.ViewModel;
using PleasanterBridge.src.APIBridge;
using PleasanterBridge.src.DataRepository.Service;
using System.Diagnostics;

namespace offlineMeeting.Controllers
{
    public class QRController : Controller
    {
        private readonly ILogger<QRController> _logger;
        private readonly IHubContext<MyHub> _hubContext;
        private readonly IPleasanterApiBridge _bridge;
        private readonly IPleasanterRepository _pleasanterRepository;
        private long CheckinSiteId;

        public QRController(
            ILogger<QRController> logger,
            IHubContext<MyHub> hubContext,
            IPleasanterApiBridge bridge,
            IPleasanterRepository pleasanterRepository
        )
        {
            _logger = logger;
            _hubContext = hubContext;
            _bridge = bridge;
            _pleasanterRepository = pleasanterRepository;
            TableIdProperty tableIdProperty = new();
            CheckinSiteId = tableIdProperty.CheckinSiteId;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            SetViewDataProcess setViewDataProcess = new();
            ViewData["UserData"] = setViewDataProcess.SetUserData(HttpContext, _pleasanterRepository);

            bool isElectron = context.HttpContext.Request.Query.ContainsKey("eid")
                && long.TryParse(context.HttpContext.Request.Query["eid"], out var eid)
                && eid != 0;
            ViewData["Electron"] = setViewDataProcess.SetElectron(isElectron);

            base.OnActionExecuting(context);
        }

        /// <summary>
        /// スタート(待機)画面を表示する。画面タッチでStartへ遷移する。
        /// ElectronのMenuからcheckinUrl(eid付き)で呼ばれる想定。
        /// </summary>
        public IActionResult Index([FromQuery] long eid)
        {
            // Electronから呼ばれているかのチェック(eidの有無で判定済み)
            ElectronEntity? electron = ViewData["Electron"] as ElectronEntity;
            if (electron?.IsElectron != true)
            {
                _logger.LogWarning("QR/Index はElectron以外から呼び出されました。eid={Eid}", eid);
            }

            // Electron経由で渡されたeidをセッションへ保持し、以降の動きをそろえる
            if (eid != 0)
            {
                ManageEventId manageEventId = new(HttpContext);
                manageEventId.SetEventId(eid);
            }

            return View();
        }

        /// <summary>
        /// スタート画面からの遷移先。StartProcessで開始前処理を行い、QR画面を呼び出す。
        /// </summary>
        public IActionResult Start([FromQuery] long eid)
        {
            StartProcess startProcess = new(HttpContext, _pleasanterRepository, CheckinSiteId);
            startProcess.Execute(eid);

            return RedirectToAction("Qr", new { eid });
        }

        /// <summary>
        /// QR(チェックイン)画面。通常呼び出しもStart経由の呼び出しも共通で使用する。
        /// </summary>
        public IActionResult Qr([FromQuery] long eid)
        {
            ManageEventId manageEventId = new(HttpContext);

            if (eid != 0)
            {
                manageEventId.SetEventId(eid);
            }

            long eventId = eid != 0 ? eid : manageEventId.GetEventId();

            GetJoinerListProcess getJoinerListProcess = new(HttpContext, _pleasanterRepository, eventId);
            var joinerList = getJoinerListProcess.Get(CheckinSiteId);

            QRViewModel viewModel = new()
            {
                EventId = eventId,
                JoinerList = joinerList
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Checkin([FromBody] CheckinRequest request)
        {
            CheckinProcess checkinProcess = new(
                httpContext: HttpContext,
                hubContext: _hubContext,
                bridge: _bridge
            );

            bool result = await checkinProcess.Execute(request.Id, request.UserCd, request.UserName, request.QrUrl);

            return Json(new { success = result });
        }

        /// <summary>
        /// Electron印刷用: ユーザ名とQRコードを表示する
        /// </summary>
        public IActionResult PrintQr([FromQuery] long eid, long userCd)
        {
            ManageEventId manageEventId = new(HttpContext);
            manageEventId.SetEventId(eid);

            GetJoinerListProcess getJoinerListProcess = new(HttpContext, _pleasanterRepository, eid);
            var joinerList = getJoinerListProcess.Get(CheckinSiteId);
            var joiner = joinerList.FirstOrDefault(j => j.UserCd == userCd.ToString());

            ViewBag.UserName = joiner?.Name ?? "";
            ViewBag.QrUrl = joiner?.Url ?? "";

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}