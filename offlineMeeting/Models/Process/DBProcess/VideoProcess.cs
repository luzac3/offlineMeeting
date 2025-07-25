using offlineMeeting.Models.DBEntity;
using offlineMeeting.Models.Share;
using Microsoft.EntityFrameworkCore;

namespace offlineMeeting.Models.Process.DBProcess
{
    public class VideoProcess : DBContext
    {
        public DbSet<VideoEntity> Video { get; set; }

        public VideoProcess() { }

        public List<VideoEntity> Get()
        {
            return Video.ToList();
        }

        public VideoEntity Get(string deviceId)
        {
            return Video.Where(x=>x.DeviceId == deviceId).FirstOrDefault() ??
                throw new Exception("invalid deviceId");
        }

        public bool Insert(VideoEntity videoEntity) { 
            try
            {
                Video.Add(videoEntity);
                    SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                throw;
            }
            return true;
        }

        public bool Update(VideoEntity videoEntity)
        {
            try
            {
                Video.Update(videoEntity);
                SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                throw;
            }
            return true;
        }

        public bool Upsert(VideoEntity videoEntity)
        {
            return true;
        }
    }
}