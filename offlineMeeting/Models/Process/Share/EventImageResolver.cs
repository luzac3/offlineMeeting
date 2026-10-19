using Microsoft.AspNetCore.Hosting;

namespace offlineMeeting.Models.Process.Share
{
    /// <summary>
    /// イベント有効化時にDBのBase64画像をSessionIDフォルダへ展開し、
    /// 以降はフォルダから画像を参照（URLパス）するためのヘルパー。
    /// </summary>
    public static class EventImageResolver
    {
        private const string BaseFolder = "eventImages";

        /// <summary>
        /// SessionIDフォルダの物理パスを返す。
        /// </summary>
        public static string GetSessionPhysicalPath(IWebHostEnvironment env, string sessionId)
        {
            return Path.Combine(env.WebRootPath, BaseFolder, sessionId);
        }

        /// <summary>
        /// 1枚の画像(Base64)をSessionIDフォルダに保存し、Web参照用のURLパスを返す。
        /// </summary>
        public static string SaveImage(IWebHostEnvironment env, string sessionId, long siteId, long resultId, int index, string base64)
        {
            string dir = Path.Combine(env.WebRootPath, BaseFolder, sessionId, siteId.ToString());
            Directory.CreateDirectory(dir);

            string fileName = $"{resultId}_{index}.jpg";
            string filePath = Path.Combine(dir, fileName);

            File.WriteAllBytes(filePath, Convert.FromBase64String(base64));

            return BuildWebPath(sessionId, siteId, resultId, index);
        }

        /// <summary>
        /// SessionIDフォルダ内に該当画像が存在すればそのURLパスを返す。無ければ null。
        /// </summary>
        public static string? ResolveImageWebPath(IWebHostEnvironment env, string? sessionId, long siteId, long resultId, int index)
        {
            if (string.IsNullOrEmpty(sessionId))
            {
                return null;
            }

            string fileName = $"{resultId}_{index}.jpg";
            string filePath = Path.Combine(env.WebRootPath, BaseFolder, sessionId, siteId.ToString(), fileName);

            return File.Exists(filePath) ? BuildWebPath(sessionId, siteId, resultId, index) : null;
        }

        /// <summary>
        /// SessionIDフォルダごと削除する。
        /// </summary>
        public static void DeleteSessionFolder(IWebHostEnvironment env, string sessionId)
        {
            if (string.IsNullOrEmpty(sessionId))
            {
                return;
            }

            string dir = GetSessionPhysicalPath(env, sessionId);
            if (Directory.Exists(dir))
            {
                Directory.Delete(dir, true);
            }
        }

        private static string BuildWebPath(string sessionId, long siteId, long resultId, int index)
        {
            return $"/{BaseFolder}/{sessionId}/{siteId}/{resultId}_{index}.jpg";
        }
    }
}
