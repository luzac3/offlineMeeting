using SkiaSharp;

namespace offlineMeeting.Models.Process.Share
{
    public static class EventImageResolver
    {
        private const string BaseFolder = "eventImages";

        // SessionIDフォルダの物理パスを返す。
        public static string GetSessionPhysicalPath(IWebHostEnvironment env, string sessionId)
        {
            return Path.Combine(env.WebRootPath, BaseFolder, sessionId);
        }

        // WebP化により画像サイズを削減し、読み込みを高速化する。
        public static string SaveImage(IWebHostEnvironment env, string sessionId, long siteId, long resultId, int index, string base64)
        {
            string dir = Path.Combine(env.WebRootPath, BaseFolder, sessionId, siteId.ToString());
            Directory.CreateDirectory(dir);

            string fileName = $"{resultId}_{index}.webp";
            string filePath = Path.Combine(dir, fileName);

            using (SKBitmap bitmap = SKBitmap.Decode(Convert.FromBase64String(base64)))
            using (SKImage image = SKImage.FromBitmap(bitmap))
            using (SKData data = image.Encode(SKEncodedImageFormat.Webp, 75))
            using (FileStream stream = File.OpenWrite(filePath))
            {
                data.SaveTo(stream);
            }

            return BuildWebPath(sessionId, siteId, resultId, index);
        }

        // SessionIDフォルダ内に該当画像が存在すればそのURLパスを返す。無ければ null。
        public static string? ResolveImageWebPath(IWebHostEnvironment env, string? sessionId, long siteId, long resultId, int index)
        {
            if (string.IsNullOrEmpty(sessionId))
            {
                return null;
            }

            string fileName = $"{resultId}_{index}.webp";
            string filePath = Path.Combine(env.WebRootPath, BaseFolder, sessionId, siteId.ToString(), fileName);

            return File.Exists(filePath) ? BuildWebPath(sessionId, siteId, resultId, index) : null;
        }

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
            return $"/{BaseFolder}/{sessionId}/{siteId}/{resultId}_{index}.webp";
        }
    }
}
