using offlineMeeting.Models.Entity.Pos.SendOrderCompletedEntity;
using System.Net;
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

        public async Task<Dictionary<string, string>> Send()
        {
            #if DEBUG
                string pleasanterUrlBase = "http://localhost:59802";
            #else
                string pleasanterUrlBase = "https://pleasanter.wolfslab.jp";
            #endif

            HttpClient httpClient = new();
            bool isError = false;
            foreach (int resultId in CompletedResultIdList)
            {
                string json = JsonSerializer.Serialize(new SendOrderCompletedEntity());
                StringContent content = new(json, System.Text.Encoding.UTF8, "application/json");
                string url = $"{pleasanterUrlBase}/api/items/{resultId}/update";
                var response = await httpClient.PostAsync(url, content);

                if (!response.StatusCode.Equals(HttpStatusCode.OK))
                {
                    isError = true;
                }
            }

            if (!isError)
            {
                return new Dictionary<string, string> { { "status", "200" }, { "message", "注文完了を更新しました" } };

            }
            else
            {
                return new Dictionary<string, string> { { "status", "500" }, { "message", "注文完了の更新に失敗しました" } };

            }
        }
    }
}