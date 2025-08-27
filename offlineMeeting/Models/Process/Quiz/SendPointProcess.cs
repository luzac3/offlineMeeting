using Microsoft.AspNetCore.SignalR;
using offlineMeeting.Models.Entity.Login;
using offlineMeeting.Models.Entity.Quiz;
using offlineMeeting.Models.Process.DBProcess;
using offlineMeeting.Models.Share;
using System.Data;
using System.Net;
using System.Text.Json;

namespace offlineMeeting.Models.Process.Quiz
{
    public class SendPointProcess : ResultsProcess
    {
        private readonly IHubContext<MyHub> _hubContext;
        private string AnswerSiteId;
        private string EventId;
        private List<PointPostEntity> PointPostEntityList;
        private HttpContext HttpContext;

        public SendPointProcess(IHubContext<MyHub> hubContext, List<PointPostEntity> pointPostEntityList, int answerSiteId, HttpContext httpContext)
        {
            _hubContext = hubContext;
            ManageEventId manageEventId = new (httpContext);
            ManageUserCd manageUserCd = new (httpContext);

            EventId = manageEventId.GetEventId();
            if (EventId == "0")
            {
                throw new DataException("イベントIDが不正です");
            }
            AnswerSiteId = answerSiteId.ToString();
            PointPostEntityList = pointPostEntityList;
            HttpContext = httpContext;
        }

        public async Task<string> Send()
        {
            #if DEBUG
                string pleasanterUrlBase = "http://localhost:59802";
            #else
                string pleasanterUrlBase = "https://pleasanter.wolfslab.jp";
            #endif

            List<SendPointEntity> sendPointEntityList = new List<SendPointEntity>();

            HttpClient httpClient = new HttpClient();
            bool flag = true;

            foreach (var pointPostEntity in PointPostEntityList)
            {

                string url = $"{pleasanterUrlBase}/api/items/{pointPostEntity.AnswerId}/update";

                if (pointPostEntity.AnswerId == null || pointPostEntity.Point == null)
                {
                    return JsonSerializer.Serialize(new Dictionary<string, string> { { "status", "500" }, { "message", "点数の送信に失敗しました" }, { "url", url } });
                }

                new SendPointEntity(
                    point: pointPostEntity.Point!
                );

                string json = JsonSerializer.Serialize(new SendPointEntity(
                    point: pointPostEntity.Point!
                ));
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(url, content);

                if (!response.StatusCode.Equals(HttpStatusCode.OK))
                {
                    flag = false;
                }
            }

            if (!flag)
            {
                return JsonSerializer.Serialize(new Dictionary<string, string> { { "status", "500" }, { "message", "点数の送信に失敗しました" }});
            }
            else
            {
                // await _hubContext.Clients.All.SendAsync("UserAnswerId", new Dictionary<string, string> { { "ResultId", resultId } });

                return JsonSerializer.Serialize(new Dictionary<string, string> { { "status", "200" }, { "message", "点数を送信しました" } });
            }
        }
    }
}