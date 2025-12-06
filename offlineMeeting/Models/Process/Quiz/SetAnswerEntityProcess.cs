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
        private GetBinariesProcess GetBinariesProcess;

        public SetAnswerEntityProcess(string eventId, int joinerSiteId, HttpContext httpContext)
        {
            EventId = eventId;
            JoinerSiteId = joinerSiteId;
            this.httpContext = httpContext;
            GetBinariesProcess = new GetBinariesProcess();
        }

        public AnswerEntity Get(string resultId)
        {
            GetUsersProcess getUsersProcess = new(JoinerSiteId, httpContext);

            ResultsEntity result = base
                .GetSingleRecord(resultId);

            return CreateAnswerEntity(result);
        }

        public List<AnswerEntity> GetList(int answerSiteId, string quizId)
        {
            GetUsersProcess getUsersProcess = new(JoinerSiteId, httpContext);
            List<ResultsEntity> results = GetFromSiteId(answerSiteId)
                .Where(x => x.ClassC == quizId)
                .OrderBy(x => x.ResultId)
                .ToList();

            // Nullチェックを追加
            ResultsEntity? resultsEntity = quizId != null ? GetSingleRecord(quizId) : null;

            List<AnswerEntity> answerEntityList = new List<AnswerEntity>();
            foreach (var result in results)
            {
                answerEntityList.Add(CreateAnswerEntity(result));
            }
            return answerEntityList;
        }
        private AnswerEntity CreateAnswerEntity(ResultsEntity result)
        {
            GetUsersProcess getUsersProcess = new(JoinerSiteId, httpContext);

            string[]? imageSplit = result!.DescriptionA?.Split("/");
            string guid = "";
            if (imageSplit != null && imageSplit?.Length > 2)
            {
                guid = string.IsNullOrEmpty(imageSplit?[2]) ? "" : imageSplit[2];
            }
            string? image = GetBinariesProcess.GetImage(guid);

            string imageBase64 = "";
            string lowImageBase64 = "";
            int width = 1475;
            int height = 1258;

            if (!string.IsNullOrEmpty(image))
            {
                try
                {
                    using ImageHelper imageHelper = new ImageHelper(Convert.FromBase64String(image));
                    (width, height) = imageHelper.GetSize();
                    imageBase64 = $"data:image/jpeg;base64,{image}";
                    lowImageBase64 = $"data:image/webp;base64,{imageHelper.GetLowQualityBase64()}";
                }
                catch
                {
                    // fixme エラー処理
                }
            }

            JoinerEntity userData = getUsersProcess.GetUserData(result.ClassB ?? "0");

            // Nullチェックを追加
            ResultsEntity? resultsEntity = result.ClassC != null ? GetSingleRecord(result.ClassC) : null;

            return new AnswerEntity(
                resultId: result.ResultId.ToString(),
                userCd: result.ClassB,
                userName: userData.UserName,
                quizId: result.ClassC,
                image: imageBase64,
                lowImage: lowImageBase64,
                width: width,
                height: height,
                maxPoint: Convert.ToInt32(resultsEntity?.NumA ?? 0),
                point: Convert.ToInt32(result.NumA),
                answerTime: result.DateA ?? DateTime.MinValue
            );
        }
    }
}