using offlineMeeting.Models.Process.GameData;
using Microsoft.AspNetCore.SignalR;

namespace offlineMeeting.Models.Hubs
{
    public class SignalRHub : Hub
    {
        public async Task GetIndex(
            int eventNumber,
            int gameNumber,
            int handNumber,
            int handSubNumber,
            string targetPath,
            bool isVideo,
            int? mediaNumber = null
        )
        {
            SendMediaPropertyProcess sendMediaPropertyProcess = new SendMediaPropertyProcess(
                eventNumber,
                gameNumber,
                handNumber,
                handSubNumber,
                isVideo,
                mediaNumber
            );

            await sendMediaPropertyProcess.GetIndexFile(targetPath, Clients);
        }

        public async Task FinishRecording(
            string eventNumber,
            string gameSubNumber,
            string targetPath
        )
        {
            Recording recording = new Recording();
            await recording.FinishRecording(eventNumber, gameSubNumber, targetPath, Clients);
        }
    }
}
