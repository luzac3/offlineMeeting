using Microsoft.Extensions.Logging;
using offlineMeeting.Models.Process.Share;

namespace offlineMeeting.Models.Process.Pos
{
    public class ResponseLogProcess
    {
        private readonly ILogger<ResponseLogProcess> _logger;
        public ResponseLogProcess(ILogger<ResponseLogProcess> logger)
        {
            _logger = logger;
        }
        public void OutputResponseLog(string status, Dictionary<string, string>  resoponse)
        {
            if(status == "200")
            {
                _logger.LogInformation(resoponse.ToString());
                CreateLogProcess.Information(resoponse?.ToString() ?? "");
            }
            else
            {
                _logger.LogError(resoponse.ToString());
                CreateLogProcess.Error(resoponse?.ToString() ?? "");
            }
        }
    }
}
