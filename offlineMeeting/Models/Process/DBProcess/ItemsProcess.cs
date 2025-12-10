using offlineMeeting.Models.DBProperty;
using offlineMeeting.Models.Share;
using Microsoft.EntityFrameworkCore;

namespace offlineMeeting.Models.Process.DBProcess
{
    public class ItemsProcess : DBContext
    {
        protected DbSet<ItemsEntity> Items { get; set; }

        protected ItemsProcess() { }

        protected List<ItemsEntity> Get()
        {
            return Items.ToList();
        }
    }
}