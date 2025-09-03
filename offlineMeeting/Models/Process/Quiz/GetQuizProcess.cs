using offlineMeeting.Models.Entity.Quiz;
using offlineMeeting.Models.Share;
using System.Data;

namespace offlineMeeting.Models.Process.Quiz
{
    public class GetQuizProcess
    {
        private string EventId;
        private SetQuizEntityProcess SetQuizEntityProcess;

        public GetQuizProcess(HttpContext httpContext, int quizSiteId) {

            ManageEventId manageEventId = new(httpContext);

            EventId = manageEventId.GetEventId();
            if (EventId == "0")
            {
                throw new DataException("イベントIDが不正です");
            }

            SetQuizEntityProcess = new(quizSiteId, EventId);
        }

        public QuizEntity? Get()
        {
            return SetQuizEntityProcess.Get();
        }

        public QuizEntity Get(string resultId)
        {
            return SetQuizEntityProcess.Get(resultId);
        }

        public List<QuizEntity> GetList(int quizSiteId)
        {
            return SetQuizEntityProcess.GetList(quizSiteId);
        }
    }
}
