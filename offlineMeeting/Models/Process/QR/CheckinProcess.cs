using Microsoft.AspNetCore.SignalR;
using offlineMeeting.Models.Entity.Login;
using offlineMeeting.Models.Share;
using PleasanterBridge.src.APIBridge;
using System.Net;

namespace offlineMeeting.Models.Process.QR
{
    public class CheckinProcess
    {
        private readonly IHubContext<MyHub> _hubContext;
        private readonly IPleasanterApiBridge _bridge;
        private readonly long _eventId;

        public CheckinProcess(
            HttpContext httpContext,
            IHubContext<MyHub> hubContext,
            IPleasanterApiBridge bridge
        )
        {
            _hubContext = hubContext;
            _bridge = bridge;
            ManageEventId manageEventId = new(httpContext);
            _eventId = manageEventId.GetEventId();
        }

        public async Task<bool> Execute(long id, long userCd, string userName, string phoneticName, string qrUrl)
        {
            // idを指定して直接更新
            var updateData = new Dictionary<string, object>
            {
                { "CheckB", true }
            };
            var response = await _bridge.Update(id, updateData);

            if (response.StatusCode != (int)HttpStatusCode.OK)
            {
                return false;
            }

            string name = string.IsNullOrEmpty(phoneticName) ? userName : phoneticName; 

            // VOICEVOX で読み上げ（補助機能のため失敗しても本処理に影響させない）
            VoiceVoxProcess voiceVoxProcess = new();
            _ = voiceVoxProcess.Execute(name + "さんが、ご来店なのだ");

            // SignalR でチェックイン完了を通知
            await _hubContext.Clients.Group($"event_{_eventId}").SendAsync("CheckinComplete", new Dictionary<string, string>
            {
                { "userCd", userCd.ToString() },
                { "userName", userName },
                { "qrUrl", qrUrl }
            });

            return true;
        }
    }
}