using offlineMeeting.Models.Entity.Login;

namespace offlineMeeting.Models.ViewModel
{
    public class RegisterViewModel
    {
        public bool RequestResult { get; set; }

        public RegisterViewModel(bool requestResult) { RequestResult = requestResult; }
    }
}