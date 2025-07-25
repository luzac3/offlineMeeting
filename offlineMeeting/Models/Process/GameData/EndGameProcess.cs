using offlineMeeting.Models.Entity.GameData;
using offlineMeeting.Models.Process.DBProcess;

namespace offlineMeeting.Models.Process.GameData
{
    public class EndGameProcess
    {
        public bool EndGame( EndGameRegisterPostEntity endGameRegisterPostEntity )
        {
            int eventNumber = endGameRegisterPostEntity.EventNumber;
            int gameNumber = endGameRegisterPostEntity.GameNumber;

            // フラグをOffにしてNewGameの登録をする
            GameDataProcess gameDataProcess = new GameDataProcess();

            gameDataProcess.Update(
                eventNumber: eventNumber,
                gameNumber: gameNumber,
                inGame: true
            );

            gameDataProcess.Insert(
                eventNumber: eventNumber, 
                gameNumber: gameNumber + 1,
                handNumber: 1,
                handSubNumber: 0,
                leachCount:0,
                gameDate: null,
                inGame: false
            );

            return true;
        }
    }
}
