using offlineMeeting.Models;
using offlineMeeting.Models.Entity.GameData;
using offlineMeeting.Models.Process.GameData;
using offlineMeeting.Models.JsonDataProperty;
using offlineMeeting.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.SignalR;
using offlineMeeting.Models.Hubs;

namespace offlineMeeting.Controllers
{
    public class ManagementStreamingController : Controller
    {
        private readonly ILogger<ManagementStreamingController> _logger;
        private readonly IHubContext<SignalRHub> _hubContext;
        private ICompositeViewEngine ViewEngine;
        private GameDataViewModel GameDataViewModel;
        private DirectionProperty DirectionProperty;
        private SetGameDataProcess SetGameDataEntityProcess;

        public ManagementStreamingController(
            ILogger<ManagementStreamingController> logger,
            ICompositeViewEngine viewEngine,
            IHubContext<SignalRHub> hubContext
        )
        {
            _logger = logger;
            ViewEngine = viewEngine;
            GameDataViewModel = new GameDataViewModel();
            DirectionProperty = new DirectionProperty();
            GameDataViewModel.setDirectionProperty(DirectionProperty);
            SetGameDataEntityProcess = new SetGameDataProcess();
            _hubContext = hubContext;
        }

        [HttpGet]
        public IActionResult Index([FromQuery] int EventNumber)
        {
            GameDataViewModel = SetGameDataEntityProcess.Set(GameDataViewModel, EventNumber);
            return View(GameDataViewModel);
        }

        [HttpPost]
        public IActionResult RecordedComentary([FromBody] RecordedMediaPostEntity recordedMediaPostEntity)
        {
            Recording recording = new Recording();
            recording.StartRecording(recordedMediaPostEntity, _hubContext, "streaming/", "SendMediaProperty");
            return Json("");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}