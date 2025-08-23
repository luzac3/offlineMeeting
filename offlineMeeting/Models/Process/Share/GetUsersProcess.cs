using offlineMeeting.Models.DBProperty;
using offlineMeeting.Models.Process.DBProcess;
using offlineMeeting.Models.Share;
using System.Data;

namespace offlineMeeting.Models.Process.Share
{
    public class GetUsersProcess: ResultsProcess
    {
        private int JoinerSiteId;
        private string EventId;
        public GetUsersProcess(int joinerSiteId, HttpContext httpContext)
        {
            ManageEventId manageEventId = new ManageEventId(httpContext);
            EventId = manageEventId.GetEventId();
            if (EventId == "0")
            {
                throw new DataException("イベントIDが不正です");
            }
            JoinerSiteId = joinerSiteId;
        }
        public Dictionary<int, string> GetUsers()
        {
            Dictionary<int, string> usersDicList = new Dictionary<int, string>();
            List<ResultsEntity> resultsEntityList = GetFromSiteId(JoinerSiteId);

            foreach (ResultsEntity resultsEntity in resultsEntityList)
            {
                usersDicList.Add(Convert.ToInt32(resultsEntity.ResultId), resultsEntity.ClassA ?? "");
            }

            return usersDicList;
        }
    }
}
