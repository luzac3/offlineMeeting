using offlineMeeting.Models.Entity.Login;
using offlineMeeting.Models.Entity.Share;
using System.Text.Json;

namespace offlineMeeting.Models.Process.Login
{
    public class SendRegisterProcess
    {
        public string UserCd;
        public SendRegisterProcess(string userCd) { 
            UserCd = userCd;
        }

        public async Task<bool> Send()
        {
            SendRegisterEntity sendRegisterEntity = new();

            ApiEntity sendPlesanterApiEntity = new(data: sendRegisterEntity);

            string json = JsonSerializer.Serialize(sendPlesanterApiEntity);

            HttpClient httpClient = new HttpClient();
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await httpClient.PostAsync($"http://localhost:59802/api/items/{UserCd}/update", content);

            return response.StatusCode.Equals(200);
        }
    }
}