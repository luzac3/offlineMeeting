using Microsoft.Extensions.Logging;
using offlineMeeting.Controllers;
using offlineMeeting.Models.Process.Share;

namespace offlineMeeting.Models.Process.Pos
{
    public class ResponseLogProcess
    {
        private readonly ILogger<PosController> _logger;
        public ResponseLogProcess(ILogger<PosController> logger)
        {
            _logger = logger;
        }
        public void OutputResponseLog(string status, string response)
        {
            if(status == "200")
            {
                _logger.LogInformation(response);
                CreateLogProcess.Information(response);
            }
            else
            {
                _logger.LogError(response);
                CreateLogProcess.Error(response);
            }
        }
    }
}
