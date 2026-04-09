using Microsoft.AspNetCore.SignalR;
using offlineMeeting.Models.Entity.Login;
using offlineMeeting.Models.Process.Share;
using System.Net;
using System.Text.Json;

namespace offlineMeeting.Models.Process.Login
{
    public class SendRegisterProcess
    {
        private readonly IHubContext<MyHub> _hubContext;
        private string EventId;
        public string UserCd;
        public SendRegisterProcess(IHubContext<MyHub> hubContext, string eventId, string userCd)
        {
            _hubContext = hubContext;
            EventId = eventId;
            UserCd = userCd;
        }

        public async Task<bool> Send()
        {
            #if DEBUG
                string urlBase = "http://localhost:58874";
                string pleasanterUrlBase = "http://localhost:59802";
            #else
                string urlBase = "https://sake.wolfslab.jp";
                string pleasanterUrlBase = "https://pleasanter.wolfslab.jp";
            #endif

            SendRegisterEntity sendRegisterEntity = new();
            string json = JsonSerializer.Serialize(sendRegisterEntity);

            HttpClient httpClient = new HttpClient();
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await httpClient.PostAsync($"{pleasanterUrlBase}/api/items/{UserCd}/update", content);

            // 通信がうまくいったら
            if (response.StatusCode.Equals(HttpStatusCode.OK))
            {
                // SignalRを使ってクライアントに通知
                await _hubContext.Clients.All.SendAsync("RegisterSuccess", new Dictionary<string, string>()
                {
                    {"url", $"{urlBase}/Login/Index?eid={EventId}&uid={UserCd}"}
                });
            }
            else
            {
                CreateLogProcess.Error("LOGIN_003", $"register failure StatusCode={response.StatusCode} UserCd={UserCd}");
            }
            return response.StatusCode.Equals(HttpStatusCode.OK);
        }
    }
}