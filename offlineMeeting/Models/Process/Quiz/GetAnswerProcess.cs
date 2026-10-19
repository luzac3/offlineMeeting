using offlineMeeting.Models.Entity.Quiz;
using offlineMeeting.Models.Share;
using PleasanterBridge.src.DataRepository.Service;
using System.Data;

namespace offlineMeeting.Models.Process.Quiz
{
    public class GetAnswerProcess
    {
        private readonly IPleasanterRepository _pleasanterRepository;
        private long EventId;
        private SetAnswerEntityProcess SetAnswerEntityProcess;

        public GetAnswerProcess(HttpContext httpContext, IPleasanterRepository pleasanterRepository, long joinerSiteId)
        {
            _pleasanterRepository = pleasanterRepository;
            ManageEventId manageEventId = new(httpContext);
            EventId = manageEventId.GetEventId();

            SetAnswerEntityProcess = new(EventId, joinerSiteId, httpContext, _pleasanterRepository);
        }

        public AnswerEntity Get(long resultId)
        {
            return SetAnswerEntityProcess.Get(resultId);
        }

        public List<AnswerEntity> GetList(long answerSiteId, long quizId)
        {
            return SetAnswerEntityProcess.GetList(answerSiteId, quizId);
        }
    }
}