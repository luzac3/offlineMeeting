using Microsoft.Extensions.Caching.Memory;
using System.Data;

namespace offlineMeeting.Models.Share
{
    public class ManageEventId
    {
        private const string CacheName = "EventId";
        HttpContext HttpContext;

        public ManageEventId(HttpContext httpContext)
        {
            HttpContext = httpContext;
        }

        public void SetEventId(long eventId)
        {
            IMemoryCache cache = SessionCacheHelper.GetCache(HttpContext);
            string key = SessionCacheHelper.BuildKey(HttpContext, CacheName);
            cache.Set(key, eventId, SessionCacheHelper.TodayJstOptions());
        }

        public long GetEventId()
        {
            IMemoryCache cache = SessionCacheHelper.GetCache(HttpContext);
            string key = SessionCacheHelper.BuildKey(HttpContext, CacheName);

            cache.TryGetValue(key, out long eventId);

            if (eventId == 0)
            {
                throw new DataException("イベントIDが不正です");
            }

            return eventId;
        }

        public void ClearEventId()
        {
            IMemoryCache cache = SessionCacheHelper.GetCache(HttpContext);
            string key = SessionCacheHelper.BuildKey(HttpContext, CacheName);
            cache.Remove(key);
        }
    }
}
