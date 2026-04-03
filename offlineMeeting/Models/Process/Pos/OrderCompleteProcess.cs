using offlineMeeting.Models.Entity.Pos.SendOrderCompletedEntity;
using offlineMeeting.Models.Entity.Share;
using offlineMeeting.Models.Share;
using PleasanterBridge.src.APIBridge;
using PleasanterBridge.src.APIBridge.Contracts;
using System.Text.Json;

namespace offlineMeeting.Models.Process.Pos
{
    public class OrderCompleteProcess
    {
        private List<int> CompletedResultIdList;
        private readonly IPleasanterApiBridge _bridge;

        public OrderCompleteProcess(List<int>? completedResultIdList, IPleasanterApiBridge bridge)
        {
            CompletedResultIdList = completedResultIdList ?? new List<int> { };
            _bridge = bridge;
        }

        public async Task<List<Dictionary<string, string>>> Send()
        {
            SendOrderCompletedEntity sendOrderCompletedEntity = new();

            var dic = CompletedResultIdList.ToDictionary(
                id => id.ToString(),
                id => (object)new { CheckA = false }
            );

            List<PleasanterApiResponse> results = await _bridge.BulkUpdate(dic);

            JsonSerializerOptions options = new JsonSerializerOptions
            {
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };

            return results.Select(x =>
                new Dictionary<string, string>()
                {
                    {"status", x.StatusCode.ToString() },
                    {"response", JsonSerializer.Serialize(x, options) }
                }
            ).ToList();
        }
    }
}