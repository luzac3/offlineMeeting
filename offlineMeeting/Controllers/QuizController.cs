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
            ViewData["UserData"] = setViewDataProcess.SetUserData(HttpContext);
            base.OnActionExecuting(context);
        }

        public IActionResult Index()
        {
            try
            {
                GetQuizProcess getQuizProcess = new(HttpContext, QuizSiteId);
                QuizViewModel quizViewModel = new QuizViewModel();
                quizViewModel.setQuizEntity(getQuizProcess.Get());

                return View(quizViewModel);
            }
            catch (Exception ex)
            {
                CreateLogProcess.Error("QUIZ_001", ex);
                throw;
            }
        }

        public IActionResult QuizList()
        {
            try
            {
                GetQuizProcess getQuizProcess = new(HttpContext, QuizSiteId);
                QuizViewModel quizViewModel = new QuizViewModel();
                quizViewModel.setQuizEntityList(getQuizProcess.GetList(QuizSiteId));

                return View(quizViewModel);
            }
            catch (Exception ex)
            {
                CreateLogProcess.Error("QUIZ_002", ex);
                throw;
            }
        }

        [HttpGet("Quiz/GetQuiz/{quizId}")]
        public async Task<IActionResult> GetQuiz(int quizId)
        {
            try
            {
                GetQuizProcess getQuizProcess = new(HttpContext, QuizSiteId);
                QuizViewModel quizViewModel = new QuizViewModel();
                quizViewModel.setQuizEntity(getQuizProcess.Get(quizId.ToString()));

                var html = await _viewRenderHelperProcess.RenderToStringAsync(this, "_Quiz", quizViewModel);
                return Content(html, "text/html");
            }
            catch (Exception ex)
            {
                CreateLogProcess.Error("QUIZ_003", ex);
                throw;
            }
        }

        [HttpGet("Quiz/Orner/{resultId}")]
        public IActionResult Orner(string resultId)
        {
            try
            {
                GetQuizProcess getQuizProcess = new(HttpContext, QuizSiteId);
                GetAnswerProcess getAnswerProcess = new(HttpContext, JoinerSiteId);
                QuizViewModel quizViewModel = new QuizViewModel();
                quizViewModel.setQuizEntity(getQuizProcess.Get(resultId));
                quizViewModel.setAnswerEntityList(getAnswerProcess.GetList(AnswerSiteId, resultId));

                return View(quizViewModel);
            }
            catch (Exception ex)
            {
                CreateLogProcess.Error("QUIZ_004", ex);
                throw;
            }
        }

        [HttpGet("Quiz/SendQuiz/{resultId}")]
        public IActionResult SendQuiz(string resultId)
        {
            try
            {
                SendQuizProcess sendQuizProcess = new(_hubContext, QuizSiteId, HttpContext);

                return Json(sendQuizProcess.Send(Convert.ToInt32(resultId)));
            }
            catch (Exception ex)
            {
                CreateLogProcess.Error("QUIZ_005", ex);
                throw;
            }
        }

        [HttpGet("Quiz/ShowQuiz/{resultId}")]
        public IActionResult ShowQuiz(string resultId)
        {
            try
            {
                GetAnswerProcess getAnswerProcess = new(HttpContext, JoinerSiteId);
                QuizViewModel quizViewModel = new QuizViewModel();
                quizViewModel.setAnswerEntityList(getAnswerProcess.GetList(AnswerSiteId, resultId));

                return View(quizViewModel);
            }
            catch (Exception ex)
            {
                CreateLogProcess.Error("QUIZ_006", ex);
                throw;
            }
        }

        [HttpGet("Quiz/GetAnswer/{resultId}")]
        public async Task<IActionResult> GetAnswer(string resultId)
        {
            try
            {
                GetAnswerProcess getAnswerProcess = new(HttpContext, JoinerSiteId);
                QuizViewModel quizViewModel = new QuizViewModel();
                quizViewModel.setAnswerEntity(getAnswerProcess.Get(resultId));

                var html = await _viewRenderHelperProcess.RenderToStringAsync(this, "_UserAnswer", quizViewModel);
                return Content(html, "text/html");
            }
            catch (Exception ex)
            {
                CreateLogProcess.Error("QUIZ_007", ex);
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> SendAnswer([FromBody] AnswerPostEntity answerPostEntity)
        {
            try
            {
                SendAnswerProcess sendAnswerProcess = new(hubContext: _hubContext, answerPostEntity: answerPostEntity, answerSiteId: AnswerSiteId, httpContext: HttpContext);
                string result = await sendAnswerProcess.Send();

                return Json(result.ToString());
            }
            catch (Exception ex)
            {
                CreateLogProcess.Error("QUIZ_008", ex);
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> SendPoints([FromBody] List<PointPostEntity> pointPostEntityList)
        {
            try
            {
                SendPointProcess sendAnswerProcess = new(hubContext: _hubContext, pointPostEntityList: pointPostEntityList, answerSiteId: AnswerSiteId, httpContext: HttpContext);
                string result = await sendAnswerProcess.Send();

                return Json(result.ToString());
            }
            catch (Exception ex)
            {
                CreateLogProcess.Error("QUIZ_009", ex);
                throw;
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}