using offlineMeeting.Models.Entity.QR;

namespace offlineMeeting.Models.ViewModel
{
    public class QRViewModel
    {
        public long EventId { get; set; }
        public List<CheckinJoinerEntity> JoinerList { get; set; }

        public QRViewModel()
        {
            JoinerList = new List<CheckinJoinerEntity>();
        }
    }
}