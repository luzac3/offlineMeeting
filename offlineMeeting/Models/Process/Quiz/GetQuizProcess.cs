using offlineMeeting.Models.Entity.Quiz;
using offlineMeeting.Models.Share;
using System.Data;

namespace offlineMeeting.Models.Process.Quiz
{
    public class GetQuizProcess
    {
        private string EventId;
        private SetQuizEntityProcess SetQuizEntityProcess;

        public GetQuizProcess(HttpContext httpContext) {

            ManageEventId manageEventId = new(httpContext);

            EventId = manageEventId.GetEventId();
            if (EventId == "0")
            {
                throw new DataException("イベントIDが不正です");
            }

            SetQuizEntityProcess = new(EventId);
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
