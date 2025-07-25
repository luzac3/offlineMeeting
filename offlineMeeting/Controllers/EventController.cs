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
    public class EventController : Controller
    {
        private readonly IHubContext<SignalRHub> _hubContext;
        private readonly ILogger<HomeController> _logger;
        private ICompositeViewEngine ViewEngine;
        private GameDataViewModel GameDataViewModel;
        private DirectionProperty DirectionProperty;
        private SetGameDataProcess SetGameDataEntityProcess;

        public EventController(
            ILogger<HomeController> logger,
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
        public IActionResult StartGame([FromBody] StartGamePostEntity startGamePostEntity)
        {
            StartGameProcess startGameProcess = new StartGameProcess();
            GameDataViewModel resultStartGame = startGameProcess.startGame(GameDataViewModel, startGamePostEntity);

            var result = startGameProcess.SetGameDataPartialView(
                ViewEngine,
                ControllerContext,
                PartialView("_GameData", resultStartGame),
                PartialView("_VideoController", resultStartGame)
            );

            return Json(result);
        }

        [HttpPost]
        public IActionResult GetCamera([FromBody] GetCameraPostEntity getCameraPostEntity)
        {
            GameDataViewModel = SetGameDataEntityProcess.Set(GameDataViewModel, getCameraPostEntity.EventNumber);
            GetCameraProcess getCameraProcess = new GetCameraProcess();
            return PartialView("_VideoController", getCameraProcess.Set(GameDataViewModel, getCameraPostEntity));
        }

        [HttpPost]
        public IActionResult GetVideoUsers([FromBody] StartVideoEntity startVideoEntity)
        {
            GetVideoUsersProcess getVideoUsersProcess = new GetVideoUsersProcess();
            return Json(getVideoUsersProcess.GetUsers(startVideoEntity));
        }

        [HttpPost]
        public IActionResult RegisterVideo([FromBody] RegisterVideoPostEntity registerVideoPostEntity)
        {
            RegisterVideoProcess registerVideoProcess = new RegisterVideoProcess();
            bool result = registerVideoProcess.Set(registerVideoPostEntity);
            return Json(result);
        }

        [HttpPost]
        public IActionResult EndGame([FromBody] EndGameRegisterPostEntity endGameRegisterPostEntity)
        {
            EndGameProcess endGameProcess = new EndGameProcess();
            bool result = endGameProcess.EndGame(endGameRegisterPostEntity);

            return Json(result);
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