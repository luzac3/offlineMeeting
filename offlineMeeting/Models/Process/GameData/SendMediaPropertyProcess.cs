using offlineMeeting.Models.DBEntity;
using offlineMeeting.Models.Entity.Share;
using offlineMeeting.Models.Hubs;
using offlineMeeting.Models.Process.DBProcess;
using Microsoft.AspNetCore.SignalR;
using System.Text.Json;

namespace offlineMeeting.Models.Process.GameData
{
    public class SendMediaPropertyProcess
    {
        int EventNumber;
        int GameNumber;
        int HandNumber;
        int HandSubNumber;
        int? MediaNumber;
        bool IsVideo;

        public SendMediaPropertyProcess(
            int eventNumber,
            int gameNumber,
            int handNumber,
            int handSubNumber,
            bool isVideo,
            int? mediaNumber = null
        )
        {
            EventNumber = eventNumber;
            GameNumber = gameNumber;
            HandNumber = handNumber;
            HandSubNumber = handSubNumber;
            MediaNumber = mediaNumber;
            IsVideo = isVideo;
        }

        public async Task SendIndexFile(
            IHubContext<SignalRHub> hubcontext,
            string m3u8FilePath,
            string targetClientName
        )
        {
            await hubcontext.Clients.All.SendAsync(targetClientName, SetJsonData(m3u8FilePath));
        }

        public async Task GetIndexFile(string targetPath, IHubCallerClients Clients)
        {
            string m3u8FilePath =
                "/media/" +
                EventNumber.ToString() + "_" +
                GameNumber.ToString() + "/" +
                targetPath +
                "now/" +
                (MediaNumber != null ? MediaNumber.ToString() : "0") + "/" + 
                "TS/" + 
                (IsVideo ? "VIDEO/" : "AUDIO/") + 
                "index.m3u8";

            // fixme Json変換した文字列が自動パースされる？　Newで作った無名クラスは展開されない？
            await Clients.All.SendAsync("SendMediaProperty", CheckExistIndexFile(m3u8FilePath));
        }

        private string CheckExistIndexFile(string m3u8FilePath)
        {
            string json = "";

            string serverFilePath =
                EnvironmentEntity.ContentRootPath +
                $@"/wwwroot" +
                m3u8FilePath;

            if (File.Exists(serverFilePath))
            {
                json = MediaNumber != null ? SetJsonUserData(m3u8FilePath) : SetJsonData(m3u8FilePath);
            }

            return json;
        }

        private string SetJsonUserData(string m3u8FilePath)
        {
            int firstDirection = 0;
            GameUsersDataProcess gameUsersDataProcess = new GameUsersDataProcess();
            InGameUsersProcess inGameUsersProcess = new InGameUsersProcess();
            UsersProcess usersProcess = new UsersProcess();

            GameUsersDataEntity userData = gameUsersDataProcess.Get(EventNumber, GameNumber, null, MediaNumber);
            int userCd = userData.UserCd;
            List<InGameUsersDataEntity>? inGameUserDataList = inGameUsersProcess.Get(EventNumber, GameNumber, userCd);
            string userName = usersProcess.Get(userCd).UserName ?? "";

            if (inGameUserDataList.Count == 0)
            {
                firstDirection = userData.Direction;
            }
            else
            {
                firstDirection = inGameUserDataList.FirstOrDefault()!.DirectionCd;
            }

            var result = new
            {
                eventNumber = EventNumber,
                gameNumber = GameNumber,
                handNumber = HandNumber,
                handSubNumber = HandSubNumber,
                mediaNumber = MediaNumber,
                userCd = userCd,
                userName = userName,
                firstDirection = firstDirection,
                direction = userData.Direction,
                m3u8FilePath = m3u8FilePath,
                isVideo = IsVideo
            };

            string json = JsonSerializer.Serialize(result);

            return json;
        }

        private string SetJsonData(string m3u8FilePath)
        {
            var result = new
            {
                eventNumber = EventNumber,
                gameNumber = GameNumber,
                handNumber = HandNumber,
                handSubNumber = HandSubNumber,
                m3u8FilePath = m3u8FilePath,
                isVideo = IsVideo
            };

            string json = JsonSerializer.Serialize(result);

            return json;
        }
    }
}
