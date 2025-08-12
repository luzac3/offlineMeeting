using offlineMeeting.Models.DBProperty;
using offlineMeeting.Models.Process.Share;
using offlineMeeting.Models.Share;
using Microsoft.EntityFrameworkCore;

namespace offlineMeeting.Models.Process.DBProcess
{
    public class ResultsProcess : DBContext
    {
        protected DbSet<ResultsEntity> Results { get; set; }

        protected ResultsProcess() { }

        protected List<ResultsEntity> Get()
        {
            return Results.ToList();
        }

        protected List<ResultsEntity> GetFromSiteId(int siteId)
        {
            var test = Results;
            return Results
                .Where(x => Convert.ToInt32(x.SiteId) == siteId).ToList();
        }

        protected ResultsEntity Get(int resultId)
        {
            return Results
                .Where(x => Convert.ToInt32(x.ResultId) == resultId)
                .FirstOrDefault() ?? throw new Exception("invalid userCd");
        }
    }
}