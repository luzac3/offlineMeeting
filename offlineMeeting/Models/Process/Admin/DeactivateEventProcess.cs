using offlineMeeting.Models.Process.Share;
using PleasanterBridge.src.APIBridge;
using PleasanterBridge.src.APIBridge.Contracts;
using PleasanterBridge.src.DataRepository.Entity;
using PleasanterBridge.src.DataRepository.Service;
using System.Net;

namespace offlineMeeting.Models.Process.Admin
{
    /// <summary>
    /// イベント無効化処理。
    /// SessionIDフォルダごと画像を削除し、イベントレコードの CheckB(有効化) と ClassB(SessionId) をクリアする。
    /// </summary>
    public class DeactivateEventProcess
    {
        private readonly IWebHostEnvironment _env;
        private readonly IPleasanterApiBridge _bridge;
        private readonly IPleasanterRepository _pleasanterRepository;
        private readonly long _eventId;

        public DeactivateEventProcess(
            IWebHostEnvironment env,
            IPleasanterApiBridge bridge,
            IPleasanterRepository pleasanterRepository,
            long eventId
        )
        {
            _env = env;
            _bridge = bridge;
            _pleasanterRepository = pleasanterRepository;
            _eventId = eventId;
        }

        /// <summary>
        /// 展開済みフォルダを削除し、イベントレコードを無効化する。
        /// </summary>
        public async Task Deactivate()
        {
            TablesEntity? eventRecord = _pleasanterRepository.Select(_eventId, true).FirstOrDefault();
            string? sessionId = eventRecord?.ClassB;

            if (!string.IsNullOrEmpty(sessionId))
            {
                EventImageResolver.DeleteSessionFolder(_env, sessionId);
            }

            Dictionary<string, object> updateData = new Dictionary<string, object>
            {
                { "ClassB", "" },
                { "CheckB", false }
            };

            PleasanterApiResponse response = await _bridge.Update(
                _eventId,
                updateData
            );

            if (response.StatusCode != (int)HttpStatusCode.OK)
            {
                throw new Exception("イベントの無効化に失敗しました");
            }
        }
    }
}
