namespace offlineMeeting.Models.Process.Share
{
    public class CreateLogProcess
    {
        static private string webLogPath = "log/web/";
        static private string electronLogPath = "log/electron/";

        static public void Information(string logData)
        {
            Write(logData, "Information", webLogPath);
        }

        static public void Error(string logData)
        {
            Write(logData, "Error", webLogPath);
        }

        static public void Error(string errorCode, string message, bool isElectron = false)
        {
            string logPath = isElectron ? electronLogPath : webLogPath;
            Write($"[{errorCode}] {message}", "Error", logPath);
        }

        static public void Error(string errorCode, Exception ex, bool isElectron = false)
        {
            string logPath = isElectron ? electronLogPath : webLogPath;
            Write($"[{errorCode}] {ex.Message} {ex.StackTrace}", "Error", logPath);
        }

        static private void Write(string logData, string logLevel, string logPath)
        {
            try
            {
                if (!Directory.Exists(logPath))
                {
                    Directory.CreateDirectory(logPath);
                }
                string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                string fileName = $"{DateTime.Now:yyyyMMdd}.log";
                string fullPath = Path.Combine(logPath, fileName);
                if (!File.Exists(fullPath))
                {
                    string createText = $"{timestamp} CreateLog" + Environment.NewLine;
                    File.WriteAllText(fullPath, createText);
                }

                File.AppendAllText(fullPath, $"{timestamp} {logLevel} {logData}\n");
            }
            catch
            {
                // ログ書き込み自体の失敗は握りつぶす
            }
        }
    }
}
