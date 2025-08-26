using Microsoft.AspNetCore.Http;
using offlineMeeting.Models.DBProperty;
using offlineMeeting.Models.Entity.Quiz;
using offlineMeeting.Models.Entity.Share;
using offlineMeeting.Models.Process.DBProcess;
using offlineMeeting.Models.Process.Share;

namespace offlineMeeting.Models.Process.Quiz
{
    public class SetAnswerEntityProcess : ResultsProcess
    {
        private string EventId;
        private int JoinerSiteId;
        private HttpContext httpContext;

        public SetAnswerEntityProcess(string eventId, int joinerSiteId, HttpContext httpContext)
        {
            EventId = eventId;
            JoinerSiteId = joinerSiteId;
            this.httpContext = httpContext;
        }

        public AnswerEntity Get(string resultId)
        {
            GetBinariesProcess getBinariesProcess = new GetBinariesProcess();
            GetUsersProcess getUsersProcess = new(JoinerSiteId, httpContext);

            ResultsEntity result = base
                .GetSingleRecord(resultId);

            string[]? imageSplit = result!.DescriptionA?.Split("/");
            string guid = "";
            if (imageSplit != null && imageSplit?.Length > 2)
            {
                guid = string.IsNullOrEmpty(imageSplit?[2]) ? "" : imageSplit[2];
            }
            string? image = getBinariesProcess.GetImage(guid);

            JoinerEntity userData = getUsersProcess.GetUserData(result.ClassB ?? "0");

            return new AnswerEntity(
                userCd: result.ClassB,
                userName: userData.UserName,
                quizId: result.ClassC,
                image: result.DescriptionA,
                answerTime: result.DateA ?? DateTime.MinValue
            );
        }

        public List<AnswerEntity> GetList(int answerSiteId, string quizId)
        {
            GetUsersProcess getUsersProcess = new(JoinerSiteId, httpContext);
            List<ResultsEntity> results = GetFromSiteId(answerSiteId)
                .Where(x=>x.ClassC == quizId)
                .ToList();
            List<AnswerEntity> answerEntityList = new List<AnswerEntity>();
            foreach (var result in results)
            {
                JoinerEntity userData = getUsersProcess.GetUserData(result.ClassB ?? "0");

                answerEntityList.Add(new AnswerEntity(
                    userCd: result.ClassB,
                    userName: userData.UserName,
                    quizId: result.ClassC,
                    image: result.DescriptionA,
                    answerTime: result.DateA ?? DateTime.MinValue
                ));
            }
            return answerEntityList;
        }
    }
}