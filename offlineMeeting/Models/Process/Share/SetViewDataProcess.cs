using offlineMeeting.Models.Entity.Share;
using offlineMeeting.Models.JsonDataProperty;
using offlineMeeting.Models.Share;

namespace offlineMeeting.Models.Process.Share
{
    public class SetViewDataProcess
    {

        public SetViewDataProcess() { }

        public JoinerEntity SetUserData(HttpContext httpContext)
        {
            TableIdProperty tableIdProperty = new();
            ManageUserCd manageUserCd = new(httpContext);

            string userCd = manageUserCd.GetUserCd();
            GetUsersProcess getUsersProcess = new GetUsersProcess(tableIdProperty.JoinerSiteId, httpContext);

            if (userCd == "0")
            {
                return getUsersProcess.GetUserData();
            }
            else
            {
                return getUsersProcess.GetUserData(userCd);
            }
        }

        public ElectronEntity SetElectron(bool isElectron = false)
        {
            return new ElectronEntity(isElectron: isElectron);
        }
    }
}
