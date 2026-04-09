using Microsoft.AspNetCore.SignalR;
using offlineMeeting.Models.Entity.Login;
using offlineMeeting.Models.Entity.Quiz;
using offlineMeeting.Models.Process.DBProcess;
using offlineMeeting.Models.Process.Share;
using offlineMeeting.Models.Share;
using System.Data;
using System.Net;
using System.Text.Json;

namespace offlineMeeting.Models.Process.Quiz
{
    public class SendQuizProcess : ResultsProcess
    {
        private readonly IHubContext<MyHub> _hubContext;
        private int QuizSiteId;
        private string EventId;
        private HttpContext HttpContext;

        public SendQuizProcess(IHubContext<MyHub> hubContext, int quizSiteId, HttpContext httpContext)
        {
            _hubContext = hubContext;
            ManageEventId manageEventId = new (httpContext);
            ManageUserCd manageUserCd = new (httpContext);

            EventId = manageEventId.GetEventId();
            if (EventId == "0")
            {
                CreateLogProcess.Error("QUIZ_017", "イベントIDが不正です");
                throw new DataException("イベントIDが不正です");
            }
            QuizSiteId = quizSiteId;
            HttpContext = httpContext;
        }

        public async Task<string> Send(int quizId)
        {
            #if DEBUG
                string pleasanterUrlBase = "http://localhost:59802";
            #else
                string pleasanterUrlBase = "https://pleasanter.wolfslab.jp";
            #endif
            
            string url = "";

            ManageUserCd manageUserCd = new ManageUserCd(HttpContext);
            string userCd = manageUserCd.GetUserCd().ToString();
            if (userCd == "0")
            {
                CreateLogProcess.Error("QUIZ_022", "ユーザコードがありません");
                return JsonSerializer.Serialize(new Dictionary<string, string> { { "status", "400" }, { "message", "ユーザコードがありません" } });
            }

            HttpClient httpClient = new HttpClient();
            HttpResponseMessage response;
            StringContent content;
            string message = "";
            SendQuizEntity sendQuizEntity = new(false);
            string json = "";

            // 古いアクティブを止める
            SetQuizEntityProcess setQuizEntityProcess = new (QuizSiteId, EventId);
            List<int> activeQuizIdList = setQuizEntityProcess.GetActiveQuizId(Convert.ToInt32(QuizSiteId));
            foreach (int activeQuizId in activeQuizIdList)
            {
                json = JsonSerializer.Serialize(sendQuizEntity);
                content = new StringContent(json, System.Text.Encoding.UTF8, "application/json"); 
                url = $"{pleasanterUrlBase}/api/items/{activeQuizId}/update";

                response = await httpClient.PostAsync(url, content);
                message = await response.Content.ReadAsStringAsync();

                if (!response.StatusCode.Equals(HttpStatusCode.OK))
                {
                    CreateLogProcess.Error("QUIZ_013", $"SendQuiz stop old quiz failure StatusCode={response.StatusCode} url={url}");
                    return JsonSerializer.Serialize(new Dictionary<string, string> { { "status", "500" }, { "message", "古いクイズの停止に失敗しました" }, { "url", url }, { "sendData", json } });
                }
            }

            sendQuizEntity = new(true);
            json = JsonSerializer.Serialize(sendQuizEntity);
            content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            url = $"{pleasanterUrlBase}/api/items/{quizId}/update";

            response = await httpClient.PostAsync(url, content);
            message = await response.Content.ReadAsStringAsync();

            if (!response.StatusCode.Equals(HttpStatusCode.OK))
            {
                CreateLogProcess.Error("QUIZ_014", $"SendQuiz start failure StatusCode={response.StatusCode} url={url}");
                return JsonSerializer.Serialize(new Dictionary<string, string> { { "status", "500" }, { "message", "クイズの開始に失敗しました" }, { "url", url }, { "sendData", json } });
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

                    await _hubContext.Clients.All.SendAsync("ReceiveQuizId", new Dictionary<string, string> { { "QuizId", quizId.ToString() } });
                }
                catch (Exception ex)
                {
                    CreateLogProcess.Error("QUIZ_011", ex);
                    resultId = "";
                }
                return JsonSerializer.Serialize(new Dictionary<string, string> { { "status", "200" }, { "message", "クイズを開始しました" }, { "ResultId", resultId } });
            }
        }
    }
}