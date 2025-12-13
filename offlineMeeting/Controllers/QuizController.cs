using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.SignalR;
using offlineMeeting.Models;
using offlineMeeting.Models.Entity.Login;
using offlineMeeting.Models.Entity.Quiz;
using offlineMeeting.Models.JsonDataProperty;
using offlineMeeting.Models.Process.Quiz;
using offlineMeeting.Models.Process.Share;
using offlineMeeting.Models.ViewModel;
using PleasanterBridge.src.APIBridge;
using PleasanterBridge.src.DataRepository.Service;
using System.Diagnostics;

namespace offlineMeeting.Controllers
{
    public class QuizController : Controller
    {
        private readonly ViewRenderHelperProcess _viewRenderHelperProcess;
        private readonly IHubContext<MyHub> _hubContext;
        private readonly ILogger<QuizController> _logger;
        private readonly IPleasanterApiBridge _bridge;
        private readonly IPleasanterRepository _pleasanterRepository;
        private int QuizSiteId;
        private int AnswerSiteId;
        private int JoinerSiteId;

        public QuizController(
            ILogger<QuizController> logger,
            IHubContext<MyHub> hubContext,
            IPleasanterApiBridge bridge,
            IPleasanterRepository pleasanterRepository,
            ICompositeViewEngine viewEngine,
            ITempDataProvider tempDataProvider,
            IServiceProvider serviceProvider
        )
        {
            _logger = logger;
            _hubContext = hubContext;
            _bridge = bridge;
            _pleasanterRepository = pleasanterRepository;
            TableIdProperty tableIdProperty = new();
            QuizSiteId = tableIdProperty.QuizSiteId;
            AnswerSiteId = tableIdProperty.AnswerSiteId;
            JoinerSiteId = tableIdProperty.JoinerSiteId;
            _viewRenderHelperProcess = new ViewRenderHelperProcess(viewEngine, tempDataProvider, serviceProvider);
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            SetViewDataProcess setViewDataProcess = new();
            ViewData["UserData"] = setViewDataProcess.SetUserData(HttpContext, _pleasanterRepository);
            base.OnActionExecuting(context);
        }

        public IActionResult Index()
        {
            GetQuizProcess getQuizProcess = new(HttpContext, _pleasanterRepository, QuizSiteId);
            QuizViewModel quizViewModel = new QuizViewModel();
            quizViewModel.setQuizEntity(getQuizProcess.Get());

            return View(quizViewModel);
        }

        public IActionResult QuizList()
        {
            GetQuizProcess getQuizProcess = new(HttpContext, _pleasanterRepository, QuizSiteId);
            QuizViewModel quizViewModel = new QuizViewModel();
            quizViewModel.setQuizEntityList(getQuizProcess.GetList(QuizSiteId));

            return View(quizViewModel);
        }

        [HttpGet("Quiz/GetQuiz/{quizId}")]
        public async Task<IActionResult> GetQuiz(int quizId)
        {
            // fixme
            GetQuizProcess getQuizProcess = new(HttpContext, _pleasanterRepository, QuizSiteId);
            QuizViewModel quizViewModel = new QuizViewModel();
            quizViewModel.setQuizEntity(getQuizProcess.Get(quizId));

            var html = await _viewRenderHelperProcess.RenderToStringAsync(this, "_Quiz", quizViewModel);
            return Content(html, "text/html");
        }

        [HttpGet("Quiz/Orner/{resultId}")]
        public IActionResult Orner(int resultId)
        {
            // fixme
            GetQuizProcess getQuizProcess = new(HttpContext, _pleasanterRepository, QuizSiteId);
            GetAnswerProcess getAnswerProcess = new(HttpContext, _pleasanterRepository, JoinerSiteId);
            QuizViewModel quizViewModel = new QuizViewModel();
            quizViewModel.setQuizEntity(getQuizProcess.Get(resultId));
            quizViewModel.setAnswerEntityList(getAnswerProcess.GetList(AnswerSiteId, resultId));

            return View(quizViewModel);
        }

        [HttpGet("Quiz/SendQuiz/{resultId}")]
        public IActionResult SendQuiz(int resultId)
        {
            // fixme
            SendQuizProcess sendQuizProcess = new(_hubContext, _bridge, _pleasanterRepository, QuizSiteId, HttpContext);

            return Json(sendQuizProcess.Send(Convert.ToInt32(resultId)));
        }

        [HttpGet("Quiz/ShowQuiz/{resultId}")]
        public IActionResult ShowQuiz(int resultId)
        {
            // fixme
            GetAnswerProcess getAnswerProcess = new(HttpContext, _pleasanterRepository, JoinerSiteId);
            QuizViewModel quizViewModel = new QuizViewModel();
            quizViewModel.setAnswerEntityList(getAnswerProcess.GetList(AnswerSiteId, resultId));

            return View(quizViewModel);
        }

        [HttpGet("Quiz/GetAnswer/{resultId}")]
        public async Task<IActionResult> GetAnswer(int resultId)
        {
            // fixme
            GetAnswerProcess getAnswerProcess = new(HttpContext, _pleasanterRepository, JoinerSiteId);
            QuizViewModel quizViewModel = new QuizViewModel();
            quizViewModel.setAnswerEntity(getAnswerProcess.Get(resultId));

            var html = await _viewRenderHelperProcess.RenderToStringAsync(this, "_UserAnswer", quizViewModel);
            return Content(html, "text/html");
        }

        [HttpPost]
        public async Task<IActionResult> SendAnswer([FromBody] AnswerPostEntity answerPostEntity)
        {
            SendAnswerProcess sendAnswerProcess = new(
                hubContext: _hubContext, 
                bridge: _bridge,
                answerPostEntity: answerPostEntity, 
                answerSiteId: AnswerSiteId, 
                httpContext: HttpContext
            );
            string result = await sendAnswerProcess.Send();

            return Json(result.ToString());
        }

        [HttpPost]
        public async Task<IActionResult> SendPoints([FromBody] List<PointPostEntity> pointPostEnties)
        {
            SendPointProcess sendAnswerProcess = new(
                hubContext: _hubContext,
                bridge: _bridge,
                pointPostEnties: pointPostEnties, 
                answerSiteId: AnswerSiteId, 
                httpContext: HttpContext
            );
            string result = await sendAnswerProcess.Send();

            return Json(result.ToString());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}