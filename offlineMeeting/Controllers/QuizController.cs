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
using System.Diagnostics;

namespace offlineMeeting.Controllers
{
    public class QuizController : Controller
    {
        private readonly ViewRenderHelperProcess _viewRenderHelperProcess;
        private readonly IHubContext<MyHub> _hubContext;
        private readonly ILogger<QuizController> _logger;
        private int QuizSiteId;
        private int AnswerSiteId;
        private int JoinerSiteId;

        public QuizController(
            ILogger<QuizController> logger,
            IHubContext<MyHub> hubContext,
            ICompositeViewEngine viewEngine,
            ITempDataProvider tempDataProvider,
            IServiceProvider serviceProvider
        )
        {
            _logger = logger;
            _hubContext = hubContext;
            TableIdProperty tableIdProperty = new();
            QuizSiteId = tableIdProperty.QuizSiteId;
            AnswerSiteId = tableIdProperty.AnswerSiteId;
            JoinerSiteId = tableIdProperty.JoinerSiteId;
            _viewRenderHelperProcess = new ViewRenderHelperProcess(viewEngine, tempDataProvider, serviceProvider);
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            SetViewDataProcess setViewDataProcess = new();
            ViewData["UserData"] = setViewDataProcess.Set(HttpContext);
            base.OnActionExecuting(context);
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult QuizList()
        {
            GetQuizProcess getQuizProcess = new(HttpContext);
            QuizViewModel quizViewModel = new QuizViewModel();
            quizViewModel.setQuizEntityList(getQuizProcess.GetList(QuizSiteId));

            return View(quizViewModel);
        }

        [HttpGet("Quiz/GetQuiz/{quizId}")]
        public async Task<IActionResult> GetQuiz(int quizId)
        {
            GetQuizProcess getQuizProcess = new(HttpContext);
            QuizViewModel quizViewModel = new QuizViewModel();
            quizViewModel.setQuizEntity(getQuizProcess.Get(quizId.ToString()));

            var html = await _viewRenderHelperProcess.RenderToStringAsync(this, "_Quiz", quizViewModel);
            return Content(html, "text/html");
        }

        [HttpGet("Quiz/Orner/{resultId}")]
        public IActionResult Orner(string resultId)
        {
            GetQuizProcess getQuizProcess = new(HttpContext);
            GetAnswerProcess getAnswerProcess = new(HttpContext, JoinerSiteId);
            QuizViewModel quizViewModel = new QuizViewModel();
            quizViewModel.setQuizEntity(getQuizProcess.Get(resultId));
            quizViewModel.setAnswerEntityList(getAnswerProcess.GetList(AnswerSiteId, resultId));

            return View(quizViewModel);
        }

        [HttpGet("Quiz/GetAnswer/{resultId}")]
        public async Task<IActionResult> GetAnswer(string resultId)
        {
            GetAnswerProcess getAnswerProcess = new(HttpContext, JoinerSiteId);
            QuizViewModel quizViewModel = new QuizViewModel();
            quizViewModel.setAnswerEntity(getAnswerProcess.Get(resultId));

            var html = await _viewRenderHelperProcess.RenderToStringAsync(this, "_UserAnswer", quizViewModel);
            return Content(html, "text/html");
        }

        [HttpPost]
        public async Task<IActionResult> SendAnswer([FromBody] AnswerPostEntity answerPostEntity)
        {
            SendAnswerProcess sendAnswerProcess = new(hubContext: _hubContext, answerPostEntity: answerPostEntity, answerSiteId: AnswerSiteId, httpContext: HttpContext);
            string result = await sendAnswerProcess.Send();

            return Json(result.ToString());
        }

        [HttpPost]
        public async Task<IActionResult> SendPoints([FromBody] List<PointPostEntity> pointPostEntityList)
        {
            SendPointProcess sendAnswerProcess = new(hubContext: _hubContext, pointPostEntityList: pointPostEntityList, answerSiteId: AnswerSiteId, httpContext: HttpContext);
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