using offlineMeeting.Models.Entity.Quiz;

namespace offlineMeeting.Models.ViewModel
{
    public class QuizViewModel
    {
        public List<QuizEntity>? QuizEntityList { get; set; }
        public QuizEntity? QuizEntity { get; set; }
        public List<AnswerEntity>? AnswerEntityList { get; set; }
        public AnswerEntity? AnswerEntity { get; set; }

        public QuizViewModel() { }
        public void setQuizEntityList(List<QuizEntity> quizEntityList)
        {
            QuizEntityList = quizEntityList;
        }
        public void setQuizEntity(QuizEntity quizEntity)
        {
            QuizEntity = quizEntity;
        }
        public void setAnswerEntityList(List<AnswerEntity> answerEntity)
        {
            AnswerEntityList = answerEntity;
        }
        public void setAnswerEntity(AnswerEntity answerEntity)
        {
            AnswerEntity = answerEntity;
        }
    }
}