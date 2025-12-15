using offlineMeeting.Models.Entity.Quiz;
using offlineMeeting.Models.Share;
using PleasanterBridge.src.DataRepository.Service;
using System.Data;

namespace offlineMeeting.Models.Process.Quiz
{
    public class GetQuizProcess
    {
        private readonly IPleasanterRepository _pleasanterRepository;
        private long EventId;
        private SetQuizEntityProcess SetQuizEntityProcess;

        public GetQuizProcess(HttpContext httpContext, IPleasanterRepository pleasanterRepository, long quizSiteId) {

            _pleasanterRepository = pleasanterRepository;
            ManageEventId manageEventId = new(httpContext);

            EventId = manageEventId.GetEventId();

            SetQuizEntityProcess = new(_pleasanterRepository, quizSiteId, EventId);
        }

        public QuizEntity? Get()
        {
            return SetQuizEntityProcess.Get();
        }

        public QuizEntity Get(long resultId)
        {
            return SetQuizEntityProcess.Get(resultId);
        }

        public List<QuizEntity> GetList(long quizSiteId)
        {
            return SetQuizEntityProcess.GetList(quizSiteId);
        }
    }
}
