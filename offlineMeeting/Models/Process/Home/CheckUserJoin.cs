using offlineMeeting.Models.DBProperty;
using offlineMeeting.Models.Process.DBProcess;
using System.Security.Cryptography.X509Certificates;

namespace offlineMeeting.Models.Process.Home
{
    public class CheckUserJoin: ResultsProcess
    {
        private strinig UserCd;
        public CheckUserJoin(int userCd) {
            UserCd = userCd;
        }

        public bool Check()
        {
            ResultsEntity resultsEntity = GetSingleRecord(UserCd);
            return resultsEntity != null;
        }
    }
}
