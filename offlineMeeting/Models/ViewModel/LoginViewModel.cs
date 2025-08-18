using offlineMeeting.Models.Entity.Login;

namespace offlineMeeting.Models.ViewModel
{
    public class LoginViewModel
    {
        public GenerateQr? GenerateQr { get; set; }

        public LoginViewModel() { }

        public void setGenerateQr(GenerateQr generateQr)
        {
            GenerateQr = generateQr;
        }
    }
}