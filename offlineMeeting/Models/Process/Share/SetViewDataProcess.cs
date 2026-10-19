using Microsoft.Extensions.Caching.Memory;
using offlineMeeting.Models.Entity.Share;
using offlineMeeting.Models.JsonDataProperty;
using offlineMeeting.Models.Share;
using PleasanterBridge.src.DataRepository.Service;

namespace offlineMeeting.Models.Process.Share
{
    public class SetViewDataProcess
    {

        public SetViewDataProcess() { }

        public JoinerEntity SetUserData(HttpContext httpContext, IPleasanterRepository pleasanterRepository)
        {
            TableIdProperty tableIdProperty = new();
            ManageUserCd manageUserCd = new(httpContext);

            long userCd = manageUserCd.GetUserCd();
            GetUsersProcess getUsersProcess = new GetUsersProcess(httpContext, pleasanterRepository, tableIdProperty.JoinerSiteId);

            if (userCd == 0)
            {
                return getUsersProcess.GetUserData();
            }
            else
            {
                return getUsersProcess.GetUserData(userCd);
            }
        }

        public ElectronEntity SetElectron(bool isElectron = false)
        {
            return new ElectronEntity(isElectron: isElectron);
        }

        /// <summary>
        /// イベント(EventId)に紐づくSessionId(ClassB)を取得し、IMemoryCacheへ同期する。
        /// EventId/UserCdと同様に持ち回るための処理。キャッシュが無ければ付与する。
        /// </summary>
        public string SetSessionId(HttpContext httpContext, IPleasanterRepository pleasanterRepository)
        {
            ManageSessionId manageSessionId = new(httpContext);

            var cache = global::offlineMeeting.Models.Share.SessionCacheHelper.GetCache(httpContext);
            string eventIdKey = global::offlineMeeting.Models.Share.SessionCacheHelper.BuildKey(httpContext, "EventId");
            if (!cache.TryGetValue(eventIdKey, out long eventId) || eventId == 0)
            {
                return manageSessionId.GetSessionId() ?? "";
            }

            string? sessionId = pleasanterRepository.Select(eventId, true).FirstOrDefault()?.ClassB;

            if (string.IsNullOrEmpty(sessionId))
            {
                // 無効化済み（SessionIdなし）の場合はキャッシュをクリア
                manageSessionId.ClearSessionId();
                return "";
            }

            manageSessionId.SetSessionId(sessionId);
            return sessionId;
        }
    }
}
