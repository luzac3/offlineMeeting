using offlineMeeting.Models.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace offlineMeeting.Models.Process.GameData
{
    public class RecordMediaProcess
    {

        public RecordMediaProcess() {}

        public async Task RecordMedia(
            int eventNumber,
            int gameNumber,
            int handNumber,
            int handSubNumber,
            int? mediaNumber,
            int mediaCount,
            int mediaMinutes,
            string base64,
            bool isVideo,
            IHubContext<SignalRHub> hubcontext,
            string numberPath,
            string tsFileDirectory,
            string tsNumberFileDirectory,
            string webmNumberFileDirectory,
            string targetClientName
        )
        {
            byte[] bytes = Convert.FromBase64String(base64);

            string m3u8FilePath = tsFileDirectory + "index.m3u8";

            // folderが存在しなければディレクトリと、配信用のM3U8ファイルを作成
            if (!Directory.Exists(webmNumberFileDirectory))
            {
                Directory.CreateDirectory(webmNumberFileDirectory);
            }
            if (!Directory.Exists(tsNumberFileDirectory))
            {
                Directory.CreateDirectory(tsNumberFileDirectory);
                GenerateM3u8(m3u8FilePath, mediaMinutes);

                // M3u8を生成した時点でファイルの読み込みが可能になるので通知を送信
                SendMediaPropertyProcess sendMediaPropertyProcess = new SendMediaPropertyProcess(
                    eventNumber,
                    gameNumber,
                    handNumber,
                    handSubNumber,
                    isVideo,
                    mediaNumber
                );
                await sendMediaPropertyProcess.SendIndexFile(hubcontext, targetClientName, m3u8FilePath);
            }

            string fileName = mediaCount.ToString().PadLeft(5, '0') + ".webm";

            if (bytes == null)
            {
                throw new NullReferenceException("invalid blob");
            }
            await System.IO.File.WriteAllBytesAsync(webmNumberFileDirectory + fileName, bytes);

            string duration = FFMpeg.TsEncode(
                tsNumberFileDirectory,
                webmNumberFileDirectory, 
                mediaCount.ToString().PadLeft(5, '0'), 
                mediaMinutes, 
                isVideo
            );

            writeM3u8(m3u8FilePath, numberPath, mediaCount.ToString().PadLeft(5, '0'), duration);
        }

        public void GenerateM3u8(string m3u8FilePath, int mediaMinutes)
        {
            using (StreamWriter writer = new StreamWriter(m3u8FilePath))
            {
                writer.WriteLine("#EXTM3U");
                writer.WriteLine("#EXT-X-VERSION:3");
                writer.WriteLine("#EXT-X-TARGETDURATION:" + (mediaMinutes + 1).ToString());
                writer.WriteLine("#EXT-X-PLAYLIST-TYPE:EVENT");
                writer.WriteLine("#EXT-X-MEDIA-SEQUENCE:0");
            }
        }

        private void writeM3u8(string m3u8FilePath, string numberPath, string videoCount, string duration)
        {
            using (StreamWriter writer = new StreamWriter(m3u8FilePath, true))
            {
                writer.WriteLine($@"#EXTINF:{duration},");
                writer.WriteLine($@"{numberPath}{videoCount}.ts");
            }
        }
    }
}