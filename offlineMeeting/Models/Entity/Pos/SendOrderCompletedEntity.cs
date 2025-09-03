using offlineMeeting.Models.Entity.Share;

namespace offlineMeeting.Models.Entity.Pos.SendOrderCompletedEntity
{
    public class SendOrderCompletedEntity: ApiEntity
    {
        public CheckHashEntity CheckHash { get; set; }

        public SendOrderCompletedEntity()
        {
            CheckHash = new CheckHashEntity();
        }
    }

    public class CheckHashEntity
    {
        public bool CheckA { get; set; }

        public CheckHashEntity()
        {
            CheckA = false;
        }
    }
}