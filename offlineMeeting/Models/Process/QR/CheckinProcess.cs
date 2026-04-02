using Microsoft.AspNetCore.SignalR;
using offlineMeeting.Models.Entity.Login;
using offlineMeeting.Models.Share;
using PleasanterBridge.src.APIBridge;
using PleasanterBridge.src.DataRepository.Entity;
using PleasanterBridge.src.DataRepository.Service;
using System.Net;

namespace offlineMeeting.Models.Process.QR
{
    public class CheckinProcess
    {
        private readonly IHubContext<MyHub> _hubContext;
        private readonly IPleasanterApiBridge _bridge;
        private readonly IPleasanterRepository _repository;
        private readonly long _eventId;

        public CheckinProcess(
            HttpContext httpContext,
            IHubContext<MyHub> hubContext,
            IPleasanterApiBridge bridge,
            IPleasanterRepository repository
        )
        {
            _hubContext = hubContext;
            _bridge = bridge;
            _repository = repository;
            ManageEventId manageEventId = new(httpContext);
            _eventId = manageEventId.GetEventId();
        }

        public async Task<bool> Execute(long siteId, long userCd, string userName, string qrUrl)
        {
            // ClassA=EventId, ClassB=UserCd でレコードを特定
            TablesEntity? target = _repository
                .Select(siteId, true)
                .Where(x => x.ClassA == _eventId.ToString())
                .Where(x => x.ClassB == userCd.ToString())
                .FirstOrDefault();

            if (target == null)
            {
                return false;
            }

            // CheckB を True に更新
            var updateEntity = new { CheckB = true };
            var response = await _bridge.Update(target.ReferenceId, updateEntity);

            if (!response.StatusCode.Equals(HttpStatusCode.OK))
            {
                return false;
            }

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