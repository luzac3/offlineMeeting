
using Microsoft.AspNetCore.SignalR;

namespace offlineMeeting.Models.Entity.Login
{
    public class MyHub : Hub
    {
        public async Task SendQuizId(string quizId)
        {
            await Clients.All.SendAsync("ReceiveQuizId", new Dictionary<string, string> { { "QuizId", quizId } });
        }
    }
}
