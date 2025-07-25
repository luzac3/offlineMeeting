using offlineMeeting.Models.Entity.Share;
using System.Diagnostics;
using System.Security.Cryptography.Xml;
using System.Threading;

namespace offlineMeeting.Models.Process.GameData
{
    public class FFMpeg
    {
        public static string TsEncode(
            string tsNumberFileDirectory,
            string webmNumberFileDirectory,
            string mediaCount,
            int mediaMinutes,
        bool isVideo
        )
        {
            string videoOption =
                "-threads 1 " +
                "-c:v h264 -crf 23 " +
                "-preset superfast " +
                "-x264-params " +
                "keyint=250:" +
                "min-keyint=25:" +
                "non-deterministic:" +
                "psy-rd=1/0:" +
                "qcomp=0.75 " +
                "-r 60 ";
            string audioOption = 
                "-acodec aac -b:a 128k " +
                "-af loudnorm=I=-18 ";

            string argument =
                $@"-i {webmNumberFileDirectory}{mediaCount}.webm " +
                (isVideo ? videoOption : audioOption) +
                $@"{tsNumberFileDirectory}{mediaCount}.ts";

            System.Diagnostics.Process? process = System.Diagnostics.Process.Start(
                new ProcessStartInfo(
                    EnvironmentEntity.ContentRootPath +
                    "/wwwroot/plugins/ffmpeg/bin/ffmpeg.exe",
                    argument
                )
                {
                    CreateNoWindow = true,
                    UseShellExecute = false,
                }
            );
            process?.WaitForExit();
            process?.Close();

            return GetDuration(tsNumberFileDirectory, mediaCount, mediaMinutes, isVideo);
        }

        private static string GetDuration(
            string tsNumberFileDirectory,
            string mediaCount,
            int mediaMinutes,
            bool isVideo
        )
        {
            string duration = "";

            string argument = 
                $@"-select_streams {(isVideo ? "v" : "a")}:0 -show_entries " +
                "stream=duration -sexagesimal " +
                " -of default=noprint_wrappers=1:nokey=1 " +
                $@"{tsNumberFileDirectory}{mediaCount}.ts";

            System.Diagnostics.Process? process = System.Diagnostics.Process.Start(
                new ProcessStartInfo(
                    EnvironmentEntity.ContentRootPath +
                    "/wwwroot/plugins/ffmpeg/bin/ffprobe.exe",
                    argument
                )
                {
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                }
            );

            if(process?.StandardOutput.ReadLine() is null)
            {
                duration = (mediaMinutes + 1).ToString() + ".000000";
            }
            else
            {
                duration = process.StandardOutput
                    .ReadLine()!
                    .Trim()
                    .Split(":")[2];
            }

            process?.WaitForExitAsync();
            process?.Close();

            return duration;
        }
    }
}
