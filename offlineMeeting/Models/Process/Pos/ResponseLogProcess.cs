using offlineMeeting.Controllers;
using offlineMeeting.Models.Process.Share;
using System.Text.Json;

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

        public void OrderCompleteLog(List<Dictionary<string, string>> reponses)
        {
            var options = new JsonSerializerOptions
            {
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };

            reponses.ForEach(response =>
            {
                OutputResponseLog(response["status"], response["response"]);
            });
        }
    }
}
