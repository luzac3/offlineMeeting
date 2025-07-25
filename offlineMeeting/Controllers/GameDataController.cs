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
    public class GameDataController : Controller
    {
        private readonly IHubContext<SignalRHub> _hubContext;
        private readonly ILogger<GameDataController> _logger;
        private ICompositeViewEngine ViewEngine;
        private GameDataViewModel GameDataViewModel;
        private DirectionProperty DirectionProperty;
        private SetGameDataProcess SetGameDataEntityProcess;

        public GameDataController(
            ILogger<GameDataController> logger,
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

        [HttpPost]
        public IActionResult RecordedVideo([FromBody] RecordedMediaPostEntity recordedMediaPostEntity)
        {
            Recording recording = new Recording();
            recording.StartRecording(recordedMediaPostEntity, _hubContext, "management/", "SendMediaProperty");
            return Json("");
        }

        [HttpPost]
        public IActionResult ShowPoint([FromBody] ShowPointPostEntity showPointPostEntity)
        {
            ShowPoint showPoint = new ShowPoint();
            var result = showPoint.Show(showPointPostEntity);

            return PartialView("_ShowPoint", result);
        }

        [HttpPost]
        public IActionResult EndHandChangeDisplay([FromBody] EndHandPostEntity endHandPostEntity)
        {
            GameDataViewModel.setEndHandPostEntity(endHandPostEntity);
            GameDataViewModel = SetGameDataEntityProcess.Set(GameDataViewModel, endHandPostEntity.EventNumber);
            return PartialView("_EndGameChangeDisplay", GameDataViewModel);
        }

        [HttpPost]
        public void EndHandRegister([FromBody] EndHandRegisterPostEntity endHandRegisterPostEntity)
        {
            // postデータを退避
            GameDataViewModel.setEndHandRegisterPostEntity(endHandRegisterPostEntity);

            // 登録処理および終了時計算処理
            EndHandProcess endHandProcess = new EndHandProcess();
            GameDataViewModel endHandResult = endHandProcess.EndHand(GameDataViewModel, endHandRegisterPostEntity);
            endHandProcess.SetGameDataPartialViewAndEntity(
                ViewEngine,
                ControllerContext,
                GameDataViewModel,
                PartialView("_GameData", endHandResult),
                PartialView("_Result", endHandResult),
                _hubContext
            );
        }

        [HttpPost]
        public IActionResult EndGame([FromBody] EndGameRegisterPostEntity endGameRegisterPostEntity)
        {
            EndGameProcess endGameProcess = new EndGameProcess();
            bool result = endGameProcess.EndGame(endGameRegisterPostEntity);

            return Json(result);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}