using offlineMeeting.Models.Entity.Quiz;
using PleasanterBridge.src.DataRepository.Entity;
using PleasanterBridge.src.DataRepository.Service;

namespace offlineMeeting.Models.Process.Quiz
{
    public class SetQuizEntityProcess
    {
        private readonly IPleasanterRepository _repository;
        private int QuizSiteId;
        private int EventId;

        public SetQuizEntityProcess(IPleasanterRepository repository, int quizSiteId, int eventId)
        {
            _repository = repository;
            QuizSiteId = quizSiteId;
            EventId = eventId;
        }


        public QuizEntity? Get()
        {
            var where = new Dictionary<string, object>
            {
                { "CheckA", false }
            };

            TablesEntity? tablesEntity = _repository.Select(QuizSiteId, where, true).FirstOrDefault();

            return SetQuizEntity(tablesEntity);

        }

        public QuizEntity Get(int resultId)
        {
            TablesEntity? tablesEntity = _repository.Select(resultId, true).FirstOrDefault();

            return SetQuizEntity(tablesEntity)!;
        }

        private QuizEntity? SetQuizEntity(TablesEntity? tablesEntity)
        {
            if (tablesEntity == null)
            {
                return null;
            }
            else
            {
                string? image = tablesEntity.DescriptionC?[0];
                QuizEntity quizEntity = new(
                    resultId: tablesEntity.ReferenceId.ToString(),
                    eventId: EventId.ToString(),
                    quiz: tablesEntity.DescriptionA != null && tablesEntity.DescriptionA.Count > 0 ? tablesEntity.DescriptionA[0] : "",
                    quizImage: image == null ? "" : $"data:image/jpeg;base64,{image}",
                    hint: tablesEntity.DescriptionD != null && tablesEntity.DescriptionD.Count > 0 ? tablesEntity.DescriptionD[0] : "",
                    maxPoint: Convert.ToInt32(tablesEntity.NumA),
                    answer: tablesEntity.ClassB,
                    explanation: tablesEntity.DescriptionB != null && tablesEntity.DescriptionB.Count > 0 ? tablesEntity.DescriptionB[0] : ""
                );

                return quizEntity;
            }
        }

        public List<QuizEntity> GetList(int quizSiteId)
        {
            List<QuizEntity> quizEntityList = new List<QuizEntity> { };

            List<TablesEntity> tablesEntities = _repository.Select(quizSiteId, true);

            return tablesEntities.Select(tablesEntity =>
            {
                return new QuizEntity(
                    resultId: tablesEntity.ReferenceId.ToString(),
                    eventId: EventId.ToString(),
                    quiz: tablesEntity.ClassC,
                    quizImage: "",
                    hint: "",
                    maxPoint: Convert.ToInt32(tablesEntity.NumA),
                    answer: tablesEntity.ClassB,
                    explanation: tablesEntity.DescriptionB != null && tablesEntity.DescriptionB.Count > 0 ? tablesEntity.DescriptionB[0] : ""
                );
            }).ToList();
        }

        public List<int> GetActiveQuizId(int quizSiteId)
        {
            var where = new Dictionary<string, object>
            {
                { "ClassA", EventId },
                { "CheckA", true }
            };

            return _repository.Select(quizSiteId, where, true)
                .Select(x => Convert.ToInt32(x.ReferenceId))
                .ToList();
        }
    }
}