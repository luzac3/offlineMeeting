using offlineMeeting.Models.Share;
using offlineMeeting.Models.Entity.EventList;
using offlineMeeting.Models.Process.DBProcess;

namespace offlineMeeting.Models.Process.EventList
{
    public class RegisterEventProcess
    {
        DBContext DBContext;

        public RegisterEventProcess()
        {
            DBContext = new DBContext();
        }

        public bool RegisterEvent(RegisterEventPostEntity registerEventPostEntity)
        {
            try
            {
                var gameDataProcess = new GameDataProcess();
                var userProcess = new UsersProcess();
                var eventUsersProcess = new EventUsersProcess();
                var eventDataProcess = new EventDataProcess();

                // 新規userを登録する
                userProcess.Insert(registerEventPostEntity.TsvUsers);

                // 登録した新規ユーザーを取得する
                var usersEntityList = userProcess.Get(registerEventPostEntity.TsvUsers);

                // 既存ユーザを追加する
                usersEntityList.AddRange(
                    userProcess.Get(registerEventPostEntity.UsersCd)
                );

                // EventDataを登録する
                eventDataProcess.Insert(eventNumber: registerEventPostEntity.EventNumber,
                    eventName: registerEventPostEntity.EventName ?? "",
                    registerEventPostEntity.EventDate
                );

                // EventUserを登録する
                eventUsersProcess.Insert(
                    eventNumber: registerEventPostEntity.EventNumber,
                    usersEntityList: usersEntityList
                );


                // GameDataを作成する
                gameDataProcess.Insert(
                    eventNumber: registerEventPostEntity.EventNumber,
                    gameNumber: 1,
                    handNumber: 1,
                    handSubNumber: 0
                );
            }
            catch(Exception e)
            {
                // fixme log出力
                Console.WriteLine( e.ToString() );
                throw;
            }

            return true;
        }
    }
}