using offlineMeeting.Models.Entity.Share;

namespace offlineMeeting.Models.Entity.Coins
{
    public class SendCoinsEntity : ApiEntity
    {
        public string ClassA { get; set; } // EventNumber
        public string ClassB { get; set; } // UserCd
        public string ClassC { get; set; } // ここのResultIdは、商品のId
        public int NumA { get; set; } // コイン数
        public int NumB { get; set; } // 金額

        public SendCoinsEntity(
            string eventNumber,
            string userCd,
            string gar,
            int coins,
            int value
        )
        {
            ClassA = eventNumber;
            ClassB = userCd;
            ClassC = gar;
            NumA = coins;
            NumB = value;
        }
    }
}
