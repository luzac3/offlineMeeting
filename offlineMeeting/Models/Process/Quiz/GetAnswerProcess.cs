using offlineMeeting.Models.Entity.Quiz;
using offlineMeeting.Models.Share;
using System.Data;

namespace offlineMeeting.Models.Process.Quiz
{
    public class GetAnswerProcess
    {
        private string EventId;
        private SetAnswerEntityProcess SetAnswerEntityProcess;

        public GetAnswerProcess(HttpContext httpContext, int joinerSiteId)
        {
            ManageEventId manageEventId = new(httpContext);
            EventId = manageEventId.GetEventId();
            if (EventId == "0")
            {
                throw new DataException("イベントIDが不正です");
            }
            SetAnswerEntityProcess = new(EventId, joinerSiteId, httpContext);
        }

        public AnswerEntity Get(string resultId)
        {
            return SetAnswerEntityProcess.Get(resultId);
        }

        public List<AnswerEntity> GetList(int answerSiteId, string quizId)
        {
            return SetAnswerEntityProcess.GetList(answerSiteId, quizId);
        }
    }
}