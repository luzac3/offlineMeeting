using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.SignalR;
using offlineMeeting.Models;
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
        private readonly IHubContext<QuizHub> _hubContext;
        private readonly ILogger<QuizController> _logger;
        private int QuizSiteId;
        private int AnswerSiteId;
        private int JoinerSiteId;

        public QuizController(
            ILogger<QuizController> logger,
            IHubContext<QuizHub> hubContext
        )
        {
            _logger = logger;
            _hubContext = hubContext;
            TableIdProperty tableIdProperty = new();
            QuizSiteId = tableIdProperty.QuizSiteId;
            AnswerSiteId = tableIdProperty.AnswerSiteId;
            JoinerSiteId = tableIdProperty.JoinerSiteId;
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
        public IActionResult GetQuiz(int quizId)
        {
            GetQuizProcess getQuizProcess = new(HttpContext);
            QuizViewModel quizViewModel = new QuizViewModel();
            quizViewModel.setQuizEntityList(getQuizProcess.GetList(QuizSiteId));

            return PartialView("_Quiz", quizViewModel);
        }

        [HttpGet("Quiz/Orner/{resultId}")]
        public IActionResult Orner(string resultId)
        {
            GetQuizProcess getQuizProcess = new(HttpContext);
            GetAnswerProcess getAnswerProcess = new(HttpContext, JoinerSiteId);
            QuizViewModel quizViewModel = new QuizViewModel();
            quizViewModel.setQuizEntity(getQuizProcess.Get(resultId));
            quizViewModel.setAnswerEntityList(getAnswerProcess.GetList(AnswerSiteId, resultId));
            // AnswerListも取得に行く

            return View(quizViewModel);
        }

        [HttpGet("Quiz/GetAnswer/{resultId}")]
        public IActionResult GetAnswer(string resultId)
        {
            GetAnswerProcess getAnswerProcess = new(HttpContext, JoinerSiteId);
            QuizViewModel quizViewModel = new QuizViewModel();
            quizViewModel.setAnswerEntity(getAnswerProcess.Get(resultId));

            return PartialView(quizViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> SendAnswer([FromBody] AnswerPostEntity answerPostEntity)
        {
            SendAnswerProcess sendAnswerProcess = new(hubContext: _hubContext, answerPostEntity: answerPostEntity, quizSiteId: QuizSiteId, httpContext: HttpContext);
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