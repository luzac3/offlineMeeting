using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using offlineMeeting.Models.Process.Admin;
using offlineMeeting.Models.Process.Share;
using offlineMeeting.Models.Share;
using PleasanterBridge.src.APIBridge;
using PleasanterBridge.src.DataRepository.Entity;
using PleasanterBridge.src.DataRepository.Service;

namespace offlineMeeting.Controllers
{
    public class AdminController : Controller
    {
        private readonly ILogger<AdminController> _logger;
        private readonly IWebHostEnvironment _env;
        private readonly IPleasanterApiBridge _bridge;
        private readonly IPleasanterRepository _pleasanterRepository;

        public AdminController(
            ILogger<AdminController> logger,
            IWebHostEnvironment env,
            IPleasanterApiBridge bridge,
            IPleasanterRepository pleasanterRepository
        )
        {
            _logger = logger;
            _env = env;
            _bridge = bridge;
            _pleasanterRepository = pleasanterRepository;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            SetViewDataProcess setViewDataProcess = new();
            ViewData["UserData"] = setViewDataProcess.SetUserData(HttpContext, _pleasanterRepository);
            ViewData["Electron"] = setViewDataProcess.SetElectron();
            ViewData["SessionId"] = setViewDataProcess.SetSessionId(HttpContext, _pleasanterRepository);
            base.OnActionExecuting(context);
        }

        public IActionResult Index()
        {
            ManageEventId manageEventId = new(HttpContext);
            long eventId = manageEventId.GetEventId();

            TablesEntity? eventRecord = _pleasanterRepository.Select(eventId, true).FirstOrDefault();

            ViewData["IsActivated"] = eventRecord?.CheckB ?? false;
            ViewData["SessionId"] = eventRecord?.ClassB ?? "";

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Activate()
        {
            ManageEventId manageEventId = new(HttpContext);
            ManageSessionId manageSessionId = new(HttpContext);
            long eventId = manageEventId.GetEventId();

            ActivateEventProcess activateEventProcess = new(_env, _bridge, _pleasanterRepository, eventId);

            try
            {
                string sessionId = await activateEventProcess.Activate();
                manageSessionId.SetSessionId(sessionId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "イベント有効化に失敗しました");
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Deactivate()
        {
            ManageEventId manageEventId = new(HttpContext);
            ManageSessionId manageSessionId = new(HttpContext);
            long eventId = manageEventId.GetEventId();

            DeactivateEventProcess deactivateEventProcess = new(_env, _bridge, _pleasanterRepository, eventId);

            try
            {
                await deactivateEventProcess.Deactivate();
                manageSessionId.ClearSessionId();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "イベント無効化に失敗しました");
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction("Index");
        }
    }
}
