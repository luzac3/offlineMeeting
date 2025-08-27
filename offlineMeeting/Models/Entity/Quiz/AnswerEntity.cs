using System.Drawing;

namespace offlineMeeting.Models.Entity.Quiz
{
    public class AnswerEntity
    {
        public string? ResultId { get; set; }
        public string? UserCd { get; set; }
        public string? UserName { get; set; }
        public string? QuizId { get; set; }
        public string? Image { get; set; }
        public int MaxPoint { get; set; }
        public int Point { get; set; }
        public DateTime? AnswerTime { get; set; }

        public AnswerEntity(
            string? resultId,
            string? userCd,
            string? userName,
            string? quizId,
            string? image,
            int maxPoint,
            int point,
            DateTime answerTime
        )
        {
            ResultId = resultId;
            UserCd = userCd;
            UserName = userName;
            QuizId = quizId;
            Image = image;
            MaxPoint = maxPoint;
            Point = point;
            AnswerTime = answerTime;
        }
    }
}