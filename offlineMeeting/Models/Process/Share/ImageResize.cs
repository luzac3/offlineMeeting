using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Webp;

namespace offlineMeeting.Models.Process.Share
{
    public static class ImageResize
    {
        public static string GetLowQualityBase64(byte[] originalImageBytes)
        {
            using var ms = ConvertToMemoryStream(originalImageBytes);
            return Convert.ToBase64String(ms.ToArray());
        }

        public static byte[] GetLowQualityBytes(byte[] originalImageBytes)
        {
            using var ms = ConvertToMemoryStream(originalImageBytes);
            return ms.ToArray();
        }

        private static MemoryStream ConvertToMemoryStream(byte[] originalImageBytes)
        {
            using var image = Image.Load(originalImageBytes);
            image.Mutate(x => x.Resize(new ResizeOptions
            {
                Size = new Size(100, 0),
                Mode = ResizeMode.Max
            }));

            var encoder = new WebpEncoder { FileFormat = WebpFileFormatType.Lossy, Quality = 10 };
            var ms = new MemoryStream();
            image.Save(ms, encoder);
            ms.Position = 0;
            return ms;
        }
    }
}
