using offlineMeeting.Models.Entity.Login;
using offlineMeeting.Models.Entity.Share;

namespace offlineMeeting.Models.ViewModel
{
    public class LoginViewModel
    {
        public GenerateQr? GenerateQr { get; set; }
        public string url { get; set; }

        public LoginViewModel() {
            #if DEBUG
                url = $"http://localhost:58874/Login/QrRegister";
            #else
                url = $"https://sake.wolfslab.jp/Login/QrRegister";
            #endif
        }

        public void setGenerateQr(GenerateQr generateQr)
        {
            GenerateQr = generateQr;
        }
    }
}