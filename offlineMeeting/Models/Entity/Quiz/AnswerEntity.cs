using System.Drawing;

namespace offlineMeeting.Models.Entity.Quiz
{
    public class AnswerEntity
    {
        public string? UserCd { get; set; }
        public string? UserName { get; set; }
        public string? QuizId { get; set; }
        public string? Image { get; set; }
        public DateTime? AnswerTime { get; set; }

        public AnswerEntity(
            string? userCd,
            string? userName,
            string? quizId,
            string? image,
            DateTime answerTime
        )
        {
            UserCd = userCd;
            UserName = userName;
            QuizId = quizId;
            Image = image;
            AnswerTime = answerTime;
        }
    }
}