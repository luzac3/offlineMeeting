using offlineMeeting.Models.Entity.Quiz;

namespace offlineMeeting.Models.ViewModel
{
    public class QuizViewModel
    {
        public List<QuizEntity>? QuizEntityList { get; set; }
        public QuizEntity? QuizEntity { get; set; }

        public QuizViewModel() { }
        public void setQuizEntityList(List<QuizEntity> quizEntityList)
        {
            QuizEntityList = quizEntityList;
        }
        public void setQuizEntity(QuizEntity quizEntity)
        {
            QuizEntity = quizEntity;
        }
    }
}