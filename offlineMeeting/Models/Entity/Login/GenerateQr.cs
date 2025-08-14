namespace offlineMeeting.Models.Entity.Login
{
    public class GenerateQr
    {
        public int UserId { get; set; }

        public GenerateQr(
            int userId
        ) {
            UserId = userId;
        }
    }
}
