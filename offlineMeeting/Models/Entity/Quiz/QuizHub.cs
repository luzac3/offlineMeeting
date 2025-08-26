using Microsoft.AspNetCore.SignalR;

namespace offlineMeeting.Models.Entity.Quiz
{
    public class QuizHub : Hub
    {
        // quiz_idを全クライアントに送信
        public async Task SendQuizId(string quizId)
        {
            await Clients.All.SendAsync("ReceiveQuizId", new Dictionary<string, string>{ { "QuizId", quizId } });
        }
    }
}
