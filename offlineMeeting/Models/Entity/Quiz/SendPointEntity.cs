using offlineMeeting.Models.Entity.Share;

namespace offlineMeeting.Models.Entity.Quiz
{
    public class SendPointEntity: ApiEntity
    {
        public int NumA { get; set; }
        public SendPointEntity(
            int point
        )
        {
            NumA = point;
        }
    }
}