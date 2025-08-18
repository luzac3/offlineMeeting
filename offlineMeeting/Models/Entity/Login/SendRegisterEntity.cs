namespace offlineMeeting.Models.Entity.Login
{
    public class SendRegisterEntity
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