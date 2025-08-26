using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using offlineMeeting.Models;
using offlineMeeting.Models.JsonDataProperty;
using offlineMeeting.Models.Process.Quiz;
using offlineMeeting.Models.Process.Share;
using offlineMeeting.Models.ViewModel;
using System.Diagnostics;

namespace offlineMeeting.Controllers
{
    public class QuizController : Controller
    {
        private readonly ILogger<QuizController> _logger;
        private int QuizSiteId;

        public QuizController(ILogger<QuizController> logger)
        {
            _logger = logger;
            TableIdProperty tableIdProperty = new();
            QuizSiteId = tableIdProperty.QuizSiteId;
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

        [HttpPost("Quiz/GetQuiz/{quizId}")]
        public IActionResult GetQuiz(int quizId)
        {
            GetQuizProcess getQuizProcess = new(HttpContext);
            QuizViewModel quizViewModel = new QuizViewModel();
            quizViewModel.setQuizEntityList(getQuizProcess.GetList(QuizSiteId));

            return PartialView("_Quiz", quizViewModel);
        }

        [HttpGet("Quiz/Orner/{resultId}")]
        public IActionResult Orner(int resultId)
        {
            GetQuizProcess getQuizProcess = new(HttpContext);
            QuizViewModel quizViewModel = new QuizViewModel();
            quizViewModel.setQuizEntity(getQuizProcess.Get(resultId));

            return View(quizViewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}