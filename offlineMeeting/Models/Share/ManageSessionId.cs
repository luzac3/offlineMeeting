using Microsoft.Extensions.Caching.Memory;

namespace offlineMeeting.Models.Share
{
    public class ManageSessionId
    {
        private const string CacheName = "SessionId";
        HttpContext HttpContext;

        public ManageSessionId(HttpContext httpContext)
        {
            HttpContext = httpContext;
        }

        public void SetSessionId(string sessionId)
        {
            IMemoryCache cache = SessionCacheHelper.GetCache(HttpContext);
            string key = SessionCacheHelper.BuildKey(HttpContext, CacheName);
            cache.Set(key, sessionId, SessionCacheHelper.TodayJstOptions());
        }

        public string? GetSessionId()
        {
            IMemoryCache cache = SessionCacheHelper.GetCache(HttpContext);
            string key = SessionCacheHelper.BuildKey(HttpContext, CacheName);

            cache.TryGetValue(key, out string? sessionId);
            return string.IsNullOrEmpty(sessionId) ? null : sessionId;
        }

        public void ClearSessionId()
        {
            IMemoryCache cache = SessionCacheHelper.GetCache(HttpContext);
            string key = SessionCacheHelper.BuildKey(HttpContext, CacheName);
            cache.Remove(key);
        }
    }
}
