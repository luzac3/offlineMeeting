using Microsoft.AspNetCore.SignalR;
using offlineMeeting.Models.Entity.Login;
using offlineMeeting.Models.Entity.Quiz;
using offlineMeeting.Models.Share;
using PleasanterBridge.src.APIBridge;
using PleasanterBridge.src.APIBridge.Contracts;
using PleasanterBridge.src.DataRepository.Service;
using System.Net;
using System.Text.Json;

namespace offlineMeeting.Models.Process.Quiz
{
    public class SendQuizProcess
    {
        private readonly IHubContext<MyHub> _hubContext;
        private readonly IPleasanterApiBridge _bridge;
        private readonly IPleasanterRepository _pleasanterRepository;
        private long QuizSiteId;
        private long EventId;
        private HttpContext HttpContext;

        public SendQuizProcess(
            IHubContext<MyHub> hubContext, 
            IPleasanterApiBridge bridge, 
            IPleasanterRepository pleasanterRepository,
            long quizSiteId, 
            HttpContext httpContext
        )
        {
            _hubContext = hubContext;
            _bridge = bridge;
            _pleasanterRepository = pleasanterRepository;
            ManageEventId manageEventId = new (httpContext);
            ManageUserCd manageUserCd = new (httpContext);

            EventId = manageEventId.GetEventId();
            QuizSiteId = quizSiteId;
            HttpContext = httpContext;
        }

        public async Task<string> Send(long quizId)
        {

            ManageUserCd manageUserCd = new ManageUserCd(HttpContext);
            long userCd = manageUserCd.GetUserCd();

            PleasanterApiResponse response;
            SendQuizEntity sendQuizEntity = new(false);

            // 古いアクティブを止める
            SetQuizEntityProcess setQuizEntityProcess = new (_pleasanterRepository, QuizSiteId, EventId);
            List<long> activeQuizIdList = setQuizEntityProcess.GetActiveQuizId(QuizSiteId);
            foreach (long activeQuizId in activeQuizIdList)
            {
                response = await _bridge.Update(
                    activeQuizId,
                    sendQuizEntity
                );

                if (response.StatusCode != (int)HttpStatusCode.OK)
                {
                    return JsonSerializer.Serialize(new Dictionary<string, string> {
                        { "status", "500" },
                        { "message", "古いクイズの停止に失敗しました" },
                        { "url", response.Url ?? "" },
                        { "sendData", response.SendData ?? "" } 
                    });
                }
            }

            response = await _bridge.Update(
                quizId,
                sendQuizEntity
            );

            if (response.StatusCode != (int)HttpStatusCode.OK)
            {
                return JsonSerializer.Serialize(new Dictionary<string, string> { 
                    { "status", "500" },
                    { "message", "クイズの開始に失敗しました" },
                    { "url", response.Url ?? "" },
                    { "sendData", response.SendData ?? "" }
                });
            }
            else
            {
                try
                {
                    await _hubContext.Clients.Group($"event_{EventId}").SendAsync("ReceiveQuizId", new Dictionary<string, string> { { "QuizId", quizId.ToString() } });
                }
                catch
                {
                    return JsonSerializer.Serialize(new Dictionary<string, string> {
                    { "status", "500" },
                    { "message", "IDを損失しました" },
                    { "url", response.Url ?? "" },
                    { "sendData", response.SendData ?? "" }
                });
                }
                return JsonSerializer.Serialize(new Dictionary<string, string> { 
                    { "status", "200" }, 
                    { "message", "クイズを開始しました" }, 
                    { "ResultId", response!.Id.ToString() } 
                });
            }
        }
    }
}