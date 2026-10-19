using SkiaSharp;

namespace offlineMeeting.Models.Process.Share
{
    public class ImageHelper : IDisposable
    {
        byte[] ImageBytes;
        SKBitmap ImageObject;
        private bool disposed = false;

        public ImageHelper(byte[] imageBytes)
        {
            ImageBytes = imageBytes;
            ImageObject = SKBitmap.Decode(imageBytes);
        }

        public string GetLowQualityBase64()
        {
            using var ms = ConvertToMemoryStream();
            return Convert.ToBase64String(ms.ToArray());
        }

        public byte[] GetLowQualityBytes()
        {
            using var ms = ConvertToMemoryStream();
            return ms.ToArray();
        }

        public (int width, int height) GetSize()
        {
            return (ImageObject.Width, ImageObject.Height);
        }   

        private MemoryStream ConvertToMemoryStream()
        {
            int targetWidth = Math.Min(100, ImageObject.Width);
            int targetHeight = (int)Math.Round(ImageObject.Height * (targetWidth / (double)ImageObject.Width));
            if (targetHeight < 1)
            {
                targetHeight = 1;
            }

            using SKBitmap resized = ImageObject.Resize(
                new SKImageInfo(targetWidth, targetHeight),
                new SKSamplingOptions(SKFilterMode.Linear, SKMipmapMode.Linear));
            using SKImage image = SKImage.FromBitmap(resized);
            using SKData data = image.Encode(SKEncodedImageFormat.Webp, 10);

            MemoryStream ms = new MemoryStream();
            data.SaveTo(ms);
            ms.Position = 0;
            return ms;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // マネージドリソースの解放
                    ImageObject?.Dispose();
                }
                // アンマネージドリソースの解放（必要ならここに記述）
                disposed = true;
            }
        }

        ~ImageHelper()
        {
            Dispose(false);
        }
    }
}
