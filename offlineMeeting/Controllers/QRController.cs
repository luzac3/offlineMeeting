using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.SignalR;
using offlineMeeting.Models;
using offlineMeeting.Models.Entity.Login;
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
        private const long CheckinSiteId = 59523;

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
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            SetViewDataProcess setViewDataProcess = new();
            ViewData["UserData"] = setViewDataProcess.SetUserData(HttpContext, _pleasanterRepository);
            base.OnActionExecuting(context);
        }

        public IActionResult Index()
        {
            GetJoinerListProcess getJoinerListProcess = new(HttpContext, _pleasanterRepository);
            var joinerList = getJoinerListProcess.Get(CheckinSiteId);

            ManageEventId manageEventId = new(HttpContext);

            QRViewModel viewModel = new()
            {
                EventId = manageEventId.GetEventId(),
                JoinerList = joinerList
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Checkin([FromBody] CheckinRequest request)
        {
            ManageEventId manageEventId = new(HttpContext);
            long eventId = manageEventId.GetEventId();

            QRViewModel viewModel = new() { EventId = eventId };
            string qrUrl = $"{viewModel.BaseUrl}?eid={eventId}&uid={request.UserCd}";

            CheckinProcess checkinProcess = new(
                httpContext: HttpContext,
                hubContext: _hubContext,
                bridge: _bridge,
                repository: _pleasanterRepository
            );

            bool result = await checkinProcess.Execute(CheckinSiteId, request.UserCd, request.UserName, qrUrl);

            return Json(new { success = result });
        }

        /// <summary>
        /// Electron印刷用: ユーザ名とQRコードを表示する
        /// </summary>
        public IActionResult PrintQr([FromQuery] long eid, long userCd)
        {
            ManageEventId manageEventId = new(HttpContext);
            manageEventId.SetEventId(eid);

            GetJoinerListProcess getJoinerListProcess = new(HttpContext, _pleasanterRepository);
            var joinerList = getJoinerListProcess.Get(CheckinSiteId);
            var joiner = joinerList.FirstOrDefault(j => j.UserCd == userCd.ToString());

            QRViewModel viewModel = new() { EventId = eid };
            string qrUrl = $"{viewModel.BaseUrl}?eid={eid}&uid={userCd}";

            ViewBag.UserName = joiner?.UserName ?? "";
            ViewBag.QrUrl = qrUrl;

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

    public class CheckinRequest
    {
        public long UserCd { get; set; }
        public string UserName { get; set; } = "";
    }
}