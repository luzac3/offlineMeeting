using Microsoft.AspNetCore.SignalR;

namespace offlineMeeting.Models.Entity.Login
{
    public class LoginHub : Hub
    {
        // 今回サーバーからクライアントへの送信は不要
        // webSocketの起動だけする
    }
}
