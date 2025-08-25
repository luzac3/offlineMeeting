using offlineMeeting.Models.Entity.Share;
using offlineMeeting.Models.JsonDataProperty;
using offlineMeeting.Models.Share;

namespace offlineMeeting.Models.Process.Share
{
    public class SetViewDataProcess
    {

        public SetViewDataProcess() { }

        public JoinerEntity Set(HttpContext httpContext)
        {
            TableIdProperty tableIdProperty = new();
            ManageUserCd manageUserCd = new(httpContext);

            string userCd = manageUserCd.GetUserCd();
            if (string.IsNullOrEmpty(userCd))
            {
                // 最終的にはエラーページへ遷移
                throw new Exception("ユーザコードが存在しません。");
            }

            GetUsersProcess getUsersProcess = new GetUsersProcess(tableIdProperty.JoinerSiteId, httpContext);
            return getUsersProcess.GetUserData(userCd);
        }
    }
}
