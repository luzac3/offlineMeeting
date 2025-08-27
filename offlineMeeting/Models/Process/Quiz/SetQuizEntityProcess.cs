using offlineMeeting.Models.DBProperty;
using offlineMeeting.Models.Entity.Quiz;
using offlineMeeting.Models.Process.DBProcess;
using offlineMeeting.Models.Process.Share;

namespace offlineMeeting.Models.Process.Quiz
{
    public class SetQuizEntityProcess : ResultsProcess
    {
        private string EventId;

        public SetQuizEntityProcess(string eventId)
        {
            EventId = eventId;
        }

        public QuizEntity Get(string resultId)
        {
            ResultsEntity result = base
                .GetSingleRecord(resultId);

            string[]? imageSplit = result!.DescriptionB?.Split("/");
            string guid = "";
            if (imageSplit != null && imageSplit?.Length > 2)
            {
                guid = string.IsNullOrEmpty(imageSplit?[2]) ? "" : imageSplit[2];
            }

            GetBinariesProcess getBinariesProcess = new GetBinariesProcess();
            string? image = getBinariesProcess.GetImage(guid);
            QuizEntity quizEntity = new (
                resultId: result.ResultId.ToString(),
                eventId: EventId,
                quiz: result.DescriptionA,
                quizImage: image == null ? "" : $"data:image/jpeg;base64,{image}",
                hint: result.DescriptionD,
                maxPoint: Convert.ToInt32(result.NumA),
                answer: result.ClassB,
                explanation: result.DescriptionB
            );

            return quizEntity;
        }

        public List<QuizEntity> GetList(int quizSiteId)
        {
            List<QuizEntity> quizEntityList = new List<QuizEntity> { };

            List<ResultsEntity> results = base
                .GetFromSiteId(quizSiteId)
                .ToList();

            foreach (ResultsEntity result in results)
            {

                GetBinariesProcess getBinariesProcess = new GetBinariesProcess();
                quizEntityList
                    .Add(
                        new QuizEntity(
                            resultId: result.ResultId.ToString(),
                            eventId: EventId,
                            quiz: result.DescriptionA,
                            quizImage: "",
                            hint: result.DescriptionD,
                            maxPoint: Convert.ToInt32(result.NumA),
                            answer: result.ClassB,
                            explanation: result.DescriptionB
                        )
                    );
            }

            return quizEntityList;
        }
    }
}