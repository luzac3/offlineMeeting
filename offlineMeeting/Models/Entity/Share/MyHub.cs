
using Microsoft.AspNetCore.SignalR;

namespace offlineMeeting.Models.Entity.Login
{
    public class MyHub : Hub
    {
        public async Task JoinEvent(string eventId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"event_{eventId}");
        }
    }
}
