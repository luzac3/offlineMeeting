using offlineMeeting.Models.Entity.Share;
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

        public async Task<PleasanterApiResponseEntity> Update(Object data, string resultId)
        {
            StringContent content = MakeJson(data);
            string url = $"{pleasanterUrlBase}/api/items/{resultId}/update";

            return await SendPost(url, content);
        }

        public async Task<PleasanterApiResponseEntity> BulkInsert<T>(IEnumerable<T> dataList, string siteId)
        {
            BulkApiEntity sendPlesanterApiEntity = new(data: dataList.Cast<object>().ToList());
            StringContent content = MakeJson(sendPlesanterApiEntity);
            string url = $"{pleasanterUrlBase}/api/items/{siteId}/bulkupsert";

            return await SendPost(url, content);
        }

        public async Task<List<PleasanterApiResponseEntity>> BulkUpdate<T>(Dictionary<string, T> dataDic)
        {
            // 各UpdateのTaskを作成
            List<Task<PleasanterApiResponseEntity>> tasks = dataDic.Select(data => Update(data.Value!, data.Key)).ToList();

            List<PleasanterApiResponseEntity> results = (await Task.WhenAll(tasks)).ToList();

            return results;
        }

        public async Task<PleasanterApiResponseEntity> Insert(Object data, string siteId)
        {
            StringContent content = MakeJson(data);
            string url = $"{pleasanterUrlBase}/api/items/{siteId}/create";

            return await SendPost(url, content);
        }

        private StringContent MakeJson(Object data)
        {
            string json = JsonSerializer.Serialize(data);
            return new(json, System.Text.Encoding.UTF8, "application/json");
        }

        private async Task<PleasanterApiResponseEntity> SendPost(string url, StringContent content)
        {
            PleasanterApiResponseEntity info = new();

            try
            {
                HttpResponseMessage response = await HttpClient.PostAsync(url, content);
                string responseBody = await response.Content.ReadAsStringAsync();
                info = AddInformation(DeserializeJson(responseBody), url, content);
            }
            catch (HttpRequestException ex)
            {
                var error = ex.Message;
            }

            return info;
        }

        private PleasanterApiResponseEntity DeserializeJson(string responseBody)
        {
            return JsonSerializer.Deserialize<PleasanterApiResponseEntity>(responseBody)
                   ?? new PleasanterApiResponseEntity();
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
