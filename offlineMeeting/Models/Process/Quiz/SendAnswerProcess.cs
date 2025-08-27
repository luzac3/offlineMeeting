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
    public class SendAnswerProcess : ResultsProcess
    {
        private readonly IHubContext<MyHub> _hubContext;
        private string AnswerSiteId;
        private string EventId;
        private AnswerPostEntity AnswerPostEntity;
        private HttpContext HttpContext;

        public SendAnswerProcess(IHubContext<MyHub> hubContext, AnswerPostEntity answerPostEntity, int answerSiteId, HttpContext httpContext)
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
            AnswerPostEntity = answerPostEntity;
            HttpContext = httpContext;
        }

        public async Task<string> Send()
        {
            #if DEBUG
                string pleasanterUrlBase = "http://localhost:59802";
            #else
                string pleasanterUrlBase = "https://pleasanter.wolfslab.jp";
            #endif

            string url = $"{pleasanterUrlBase}/api/items/{AnswerSiteId}/create";

            ManageUserCd manageUserCd = new ManageUserCd(HttpContext);
            string userCd = manageUserCd.GetUserCd().ToString();
            if (userCd == "0")
            {
                return JsonSerializer.Serialize(new Dictionary<string, string> { { "status", "400" }, { "message", "ユーザコードがありません" } });
            }

            string base64Image = string.Empty;
            if (!string.IsNullOrEmpty(AnswerPostEntity.Image))
            {
                int base64Index = AnswerPostEntity.Image.IndexOf("base64,");
                if (base64Index >= 0)
                {
                    base64Image = AnswerPostEntity.Image.Substring(base64Index + 7);
                }
            }

            SendAnswerEntity sendAnswerEntity = new
            (
                eventNumber: EventId,
                userCd: userCd,
                quizId: AnswerPostEntity.QuizId != null ? AnswerPostEntity.QuizId.ToString() : string.Empty,
                base64Image: base64Image
            );

            string json = JsonSerializer.Serialize(sendAnswerEntity);

            HttpClient httpClient = new HttpClient();
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(url, content);
            var message = await response.Content.ReadAsStringAsync();

            if (!response.StatusCode.Equals(HttpStatusCode.OK))
            {
                return JsonSerializer.Serialize(new Dictionary<string, string> { { "status", "500" }, { "message", "注文の送信に失敗しました" }, { "url", url }, { "sendData", json } });
            }
            else
            {
                string resultId = "";
                var responseBody = await response.Content.ReadAsStringAsync();
                try
                {
                    using var doc = JsonDocument.Parse(responseBody);
                    if (doc.RootElement.TryGetProperty("Id", out var idProp))
                    {
                        resultId = idProp.GetRawText();
                    }
                    else if (doc.RootElement.TryGetProperty("ResultId", out var resultIdProp))
                    {
                        resultId = resultIdProp.GetRawText();
                    }

                    await _hubContext.Clients.All.SendAsync("UserAnswerId", new Dictionary<string, string> { { "ResultId", resultId } });
                }
                catch
                {
                    resultId = "";
                }

                return JsonSerializer.Serialize(new Dictionary<string, string> { { "status", "200" }, { "message", "回答を送信しました" },  { "ResultId", resultId } });
            }
        }
    }
}