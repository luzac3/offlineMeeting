using offlineMeeting.Models.Entity.Share;

namespace offlineMeeting.Models.ViewModel
{
    public class QRViewModel
    {
        public string BaseUrl { get; set; }
        public long EventId { get; set; }
        public List<JoinerEntity> JoinerList { get; set; }

        public QRViewModel()
        {
            #if DEBUG
                BaseUrl = "http://localhost:58874/Login/Index";
            #else
                BaseUrl = "https://sake.wolfslab.jp/Login/Index";
            #endif
            JoinerList = new List<JoinerEntity>();
        }
    }
}