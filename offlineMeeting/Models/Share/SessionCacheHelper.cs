using Microsoft.Extensions.Caching.Memory;

namespace offlineMeeting.Models.Share
{
    /// <summary>
    /// ASP.NET Core のセッションIDをキーに利用して、ユーザーごとの値を IMemoryCache へ保存・取得するための共通ヘルパー。
    /// 従来 Cookie に保持していた UserCd / EventId / SessionId をサーバー側メモリへ移行するために使用する。
    /// キャッシュの有効期限は「アクセスした日の当日限り（日本時間の 23:59:59 まで）」とする。
    /// </summary>
    public static class SessionCacheHelper
    {
        private const string SessionInitKey = "_init";

        /// <summary>
        /// 指定した HttpContext からスコープされた IMemoryCache を取得する。
        /// </summary>
        public static IMemoryCache GetCache(HttpContext httpContext)
        {
            return httpContext.RequestServices.GetRequiredService<IMemoryCache>();
        }

        /// <summary>
        /// セッションIDと項目名から、キャッシュ用のキーを生成する。
        /// セッションIDを確定させるため、未初期化の場合はセッションへ初期値を書き込む。
        /// </summary>
        public static string BuildKey(HttpContext httpContext, string name)
        {
            EnsureSession(httpContext);
            return $"{httpContext.Session.Id}:{name}";
        }

        /// <summary>
        /// 日本時間での当日末（23:59:59）を絶対期限とするキャッシュオプションを返す。
        /// </summary>
        public static MemoryCacheEntryOptions TodayJstOptions()
        {
            TimeZoneInfo jst = GetJstTimeZone();
            DateTimeOffset nowJst = TimeZoneInfo.ConvertTime(DateTimeOffset.UtcNow, jst);
            DateTimeOffset endOfDayJst = new DateTimeOffset(
                nowJst.Year, nowJst.Month, nowJst.Day, 23, 59, 59, nowJst.Offset);

            return new MemoryCacheEntryOptions
            {
                AbsoluteExpiration = endOfDayJst
            };
        }

        private static void EnsureSession(HttpContext httpContext)
        {
            if (string.IsNullOrEmpty(httpContext.Session.GetString(SessionInitKey)))
            {
                httpContext.Session.SetString(SessionInitKey, "1");
            }
        }

        private static TimeZoneInfo GetJstTimeZone()
        {
            try
            {
                return TimeZoneInfo.FindSystemTimeZoneById("Tokyo Standard Time");
            }
            catch (TimeZoneNotFoundException)
            {
                return TimeZoneInfo.FindSystemTimeZoneById("Asia/Tokyo");
            }
        }
    }
}
