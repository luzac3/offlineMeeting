using Microsoft.AspNetCore.SignalR;
using Microsoft.Data.SqlClient;
using offlineMeeting.Models.Entity.Login;

namespace offlineMeeting.Models.Process.Pos
{
    // オーダーを取得するように変更し、必要がない場合は商品のみを取得する
    public class GetUsersOrderTableWatcherProcess : TableWatcherProcess
    {
        private HttpContext _httpContext;
        private IHubContext<MyHub> _hubContext;

        public GetUsersOrderTableWatcherProcess(
            string sqlCommand,
            HttpContext httpContext,
            IHubContext<MyHub> hubContext
        ) : base(sqlCommand)
        {
            _httpContext = httpContext;
            _hubContext = hubContext;
            RegisterDependency();
        }

        protected override void OnSqlDependencyChange(object sender, SqlNotificationEventArgs e)
        {
            // insertかupdateの判別はいる
            // 想定としてUpdateで変わる値は一つしかない
            // 再登録
            RegisterDependency(e);
        }

        protected override void GetReader(SqlDataReader reader, SqlNotificationEventArgs? e)
        {
            if(e == null)
            {
                return;
            }

            int resultId = reader.GetInt32(2);

            if (e.Info == SqlNotificationInfo.Insert)
            {
                // keyを元に子テーブルを取得して送る
                // "0"とResultId、子テーブルのリストを通知
            }
            else if(e.Info == SqlNotificationInfo.Update)
            {
                // "1"とResultIdを通知
            }

            // DB更新を検知したら全クライアントに通知
            _hubContext.Clients.All.SendAsync("OrderTableChanged");

        }
    }
}