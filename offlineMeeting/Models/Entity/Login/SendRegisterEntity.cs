using offlineMeeting.Models.Entity.Share;

namespace offlineMeeting.Models.Entity.Login
{
    public class SendRegisterEntity: ApiEntity
    {
        public CheckHashEntity CheckHash { get; set; }

        public SendRegisterEntity()
        {
            CheckHash = new CheckHashEntity();
        }
    }

    public class CheckHashEntity
    {
        public bool CheckA { get; set; }
        public CheckHashEntity()
        {
            CheckA = true;
        }
    }
}