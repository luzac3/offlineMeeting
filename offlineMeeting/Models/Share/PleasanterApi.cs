using offlineMeeting.Models.Entity.Share;
using System.Security.Policy;
using System.Text.Json;

namespace offlineMeeting.Models.Share
{
    public class PleasanterApi
    {
        #if DEBUG
            private readonly string pleasanterUrlBase = "http://localhost:59802";
        #else
            private readonly string  pleasanterUrlBase = "https://pleasanter.wolfslab.jp";
        #endif
        
        HttpClient HttpClient;

        public PleasanterApi()
        {
            HttpClient = new();
        }

        public async Task<HttpResponseMessage> Update(Object data, string resultId)
        {
            StringContent content = MakeJson(data);
            string url = $"{pleasanterUrlBase}/api/items/{resultId}/update";

            return await HttpClient.PostAsync(url, content);
        }

        public async Task<PleasanterApiResponseEntity> BulkInsert<T>(IEnumerable<T> dataList, string siteId)
        {
            BulkApiEntity sendPlesanterApiEntity = new(data: dataList.Cast<object>().ToList());
            StringContent content = MakeJson(sendPlesanterApiEntity);
            string url = $"{pleasanterUrlBase}/api/items/{siteId}/bulkupsert";

            return await HttpClient.PostAsync(url, content);
        }

        public async Task<PleasanterApiResponseEntity> Insert(Object data, string siteId)
        {
            StringContent content = MakeJson(data);
            string url = $"{pleasanterUrlBase}/api/items/{siteId}/create";

            return AddInformation(await HttpClient.PostAsync(url, content), url, content);
        }

        private StringContent MakeJson(Object data)
        {
            string json = JsonSerializer.Serialize(data);
            return new(json, System.Text.Encoding.UTF8, "application/json");
        }

        private PleasanterApiResponseEntity AddInformation(
            PleasanterApiResponseEntity pleasanterApiResponseEntity,
            string url,
            StringContent content
        )
        {
            pleasanterApiResponseEntity.Url = url;
            pleasanterApiResponseEntity.SendData = content.ToString();

            return pleasanterApiResponseEntity;
        }
    }
}
