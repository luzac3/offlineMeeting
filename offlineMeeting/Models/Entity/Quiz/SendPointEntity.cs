using offlineMeeting.Models.Entity.Share;

namespace offlineMeeting.Models.Entity.Quiz
{
    public class SendPointEntity: ApiEntity
    {
        public NumHashEntity NumHash { get; set; }
        public SendPointEntity(
            string point
        )
        {
            NumHash = new NumHashEntity(point);
        }

        public class NumHashEntity
        {
            public string NumA { get; set; }
            public NumHashEntity(
                string point
            )
            {
                NumA = point;
            }
        }
    }
}