using offlineMeeting.Models.DBProperty;
using offlineMeeting.Models.Share;
using Microsoft.EntityFrameworkCore;

namespace offlineMeeting.Models.Process.DBProcess
{
    public class ResultsProcess : DBContext
    {
        protected DbSet<ResultsEntity> Results { get; set; }

        protected ResultsProcess() { }

        protected List<ResultsEntity> GetAll()
        {
            return Results.ToList();
        }

        protected List<ResultsEntity> GetFromSiteId(int siteId)
        {
            return Results
                .Where(x => Convert.ToInt32(x.SiteId) == siteId).ToList();
        }

        protected List<ResultsEntity> GetFromSiteId(int siteId, List<int> resultIdList)
        {
            return Results
                .Where(x => Convert.ToInt32(x.SiteId) == siteId)
                .Where(y => resultIdList.Contains(Convert.ToInt32(y.ResultId)))
                .ToList();
        }

        protected ResultsEntity GetSingleRecord(string resultId)
        {
            return Results
                .Where(x => Convert.ToInt32(x.ResultId) == Convert.ToInt32(resultId))
                .FirstOrDefault() ?? throw new Exception("invalid resultId");
        }

        protected List<ResultsEntity> GetFromResultIdList(List<int> resultIdList)
        {
            return Results
                .Where(y => resultIdList.Contains(Convert.ToInt32(y.ResultId)))
                .ToList();
        }
    }
}