using offlineMeeting.Models.Entity.Pos.SendOrderCompletedEntity;
using offlineMeeting.Models.Entity.Share;
using offlineMeeting.Models.Share;
using System.Collections.Generic;
using System.Text.Json;

namespace offlineMeeting.Models.Process.Pos
{
    public class OrderCompleteProcess
    {
        private List<int> CompletedResultIdList;

        public OrderCompleteProcess(List<int>? completedResultIdList)
        {
            CompletedResultIdList = completedResultIdList ?? new List<int> { };
        }

        public async Task<List<Dictionary<string, string>>> Send()
        {
            PleasanterApi pleasanterApi = new();

            var dic = CompletedResultIdList.ToDictionary(
                id => id.ToString(),
                id => new { CheckA = false }
            );

            List<PleasanterApiResponseEntity> results = await pleasanterApi.BulkUpdate(dic);

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