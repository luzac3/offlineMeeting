using Microsoft.Extensions.Caching.Memory;
using System.Data;
using System.Text.Json;

namespace offlineMeeting.Models.Share
{
    public class ManageUserCd
    {
        private const string CacheName = "UserCd";
        HttpContext HttpContext;

        public ManageUserCd(HttpContext httpContext)
        {
            HttpContext = httpContext;
        }

        public void SetUserCd(long userCd)
        {
            IMemoryCache cache = SessionCacheHelper.GetCache(HttpContext);
            string key = SessionCacheHelper.BuildKey(HttpContext, CacheName);
            cache.Set(key, userCd, SessionCacheHelper.TodayJstOptions());
        }

        public long GetUserCd()
        {
            IMemoryCache cache = SessionCacheHelper.GetCache(HttpContext);
            string key = SessionCacheHelper.BuildKey(HttpContext, CacheName);

            if (cache.TryGetValue(key, out long userCd))
            {
                if (userCd == 0)
                {
                    throw new DataException("ユーザコードがありません");
                }
                return userCd;
            }
            return 0;
        }

        public void ClearUserCd()
        {
            IMemoryCache cache = SessionCacheHelper.GetCache(HttpContext);
            string key = SessionCacheHelper.BuildKey(HttpContext, CacheName);
            cache.Remove(key);
        }
    }
}
