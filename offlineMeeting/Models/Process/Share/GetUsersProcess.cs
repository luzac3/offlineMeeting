using offlineMeeting.Models.DBProperty;
using offlineMeeting.Models.Entity.Share;
using offlineMeeting.Models.Process.DBProcess;
using offlineMeeting.Models.Share;
using System.Data;

namespace offlineMeeting.Models.Process.Share
{
    public class GetUsersProcess: ResultsProcess
    {
        private int JoinerSiteId;
        public GetUsersProcess(int joinerSiteId, HttpContext httpContext)
        {
            ManageEventId manageEventId = new ManageEventId(httpContext);
            if (manageEventId.GetEventId() == "0")
            {
                throw new DataException("イベントIDが不正です");
            }
            JoinerSiteId = joinerSiteId;
        }
        public JoinerEntity GetUserData(string myUserCd)
        {
            ResultsEntity resultsEntity = GetSingleRecord(myUserCd);

            return new JoinerEntity(
                eventId: resultsEntity.ClassD ?? "",
                userCd: resultsEntity.ResultId.ToString(),
                userName: resultsEntity.ClassA ?? "",
                sex: resultsEntity.ClassB ?? "",
                contact: resultsEntity.ClassC ?? "",
                handoverNotes : resultsEntity.DescriptionA ?? "",
                coins: Convert.ToInt32(resultsEntity.NumA ?? 0),
                exp: Convert.ToInt32(resultsEntity.NumB ?? 0),
                expNext: Convert.ToInt32(resultsEntity.NumC ?? 0),
                level: Convert.ToInt32(resultsEntity.NumC ?? 1),
                isAdmin: resultsEntity.CheckB ?? false
            );
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
