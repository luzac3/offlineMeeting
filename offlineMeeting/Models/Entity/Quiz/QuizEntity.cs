namespace offlineMeeting.Models.Entity.Quiz
{
    public class QuizEntity
    {
        public string? ResultId { get; set; }
        public string? EventId { get; set; }
        public string? Quiz { get; set; }
        public string? QuizImage { get; set; }
        public string? Hint { get; set; }
        public int MaxPoint { get; set; }
        public string? Answer { get; set; }
        public string? Explanation { get; set; }

        public QuizEntity(
            string? resultId,
            string? eventId,
            string? quiz,
            string? quizImage,
            string? hint,
            int maxPoint,
            string? answer,
            string? explanation
        )
        {
            ResultId = resultId;
            EventId = eventId;
            Quiz = quiz;
            QuizImage = quizImage;
            Hint = hint;
            MaxPoint = maxPoint;
            Answer = answer;
            Explanation = explanation;
        }
    }
}