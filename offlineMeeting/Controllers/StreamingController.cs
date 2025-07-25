using offlineMeeting.Models;
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
    public class StreamingController : Controller
    {
        private readonly ILogger<StreamingController> _logger;
        private readonly IHubContext<SignalRHub> _hubContext;
        private ICompositeViewEngine ViewEngine;
        private GameDataViewModel GameDataViewModel;
        private DirectionProperty DirectionProperty;
        private SetGameDataProcess SetGameDataEntityProcess;

        public StreamingController(
            ILogger<StreamingController> logger,
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}