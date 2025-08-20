using Microsoft.AspNetCore.SignalR;
using offlineMeeting.Models.Entity.Login;
using offlineMeeting.Models.Entity.Share;
using offlineMeeting.Models.Share;
using System.Net;
using System.Text.Json;

namespace offlineMeeting.Models.Process.Login
{
    public class SendRegisterProcess
    {
        private readonly IHubContext<LoginHub> _hubContext;
        public string UserCd;
        public SendRegisterProcess(IHubContext<LoginHub> hubContext, string userCd)
        {
            _hubContext = hubContext;
            UserCd = userCd;
        }

        public async Task<bool> Send()
        {
            #if DEBUG
                string urlBase = "http://localhost:58874";
            #else
                string urlBase = "https://sake.wolfslab.jp";
            #endif

            SendRegisterEntity sendRegisterEntity = new();
            string json = JsonSerializer.Serialize(sendRegisterEntity);

            HttpClient httpClient = new HttpClient();
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await httpClient.PostAsync($"{urlBase}/api/items/{UserCd}/update", content);

            // 通信がうまくいったら
            if (response.StatusCode.Equals(HttpStatusCode.OK))
            {
                // SignalRを使ってクライアントに通知
                await _hubContext.Clients.All.SendAsync("RegisterSuccess", new Dictionary<string, string>()
                {
                    {"url", $"{urlBase}/Login/Index"}
                });
            }   

            return response.StatusCode.Equals(HttpStatusCode.OK);
        }
    }
}