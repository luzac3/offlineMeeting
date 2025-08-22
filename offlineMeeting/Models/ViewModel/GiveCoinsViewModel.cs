namespace offlineMeeting.Models.ViewModel
{
    public class GiveCoinsViewModel
    {
        public Dictionary<int, string> Users { get; set; }

        public GiveCoinsViewModel(
            Dictionary<int, string> users
        )
        {
            Users = users;
        }
    }
}