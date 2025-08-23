using offlineMeeting.Models.Entity.Share;

namespace offlineMeeting.Models.Entity.Coins
{
    public class SendCoinsEntity : ApiEntity
    {
        public ClassHashEntity ClassHash { get; set; }
        public NumHashEntity NumHash { get; set; }

        public SendCoinsEntity(
            string eventNumber,
            string userCd,
            string gar,
            int coins,
            int value
        )
        {
            ClassHash = new ClassHashEntity(
                eventNumber: eventNumber,
                userCd: userCd,
                gar: gar
            );
            NumHash = new NumHashEntity(
                coins: coins,
                value: value
            );
        }
    }

    public class ClassHashEntity
    {
        public string ClassA { get; set; } // EventNumber
        public string ClassB { get; set; } // UserCd
        public string ClassC { get; set; } // ここのResultIdは、商品のId
        public ClassHashEntity(
            string eventNumber,
            string userCd,
            string gar
        )
        {
            ClassA = eventNumber;
            ClassB = userCd;
            ClassC = gar;
        }
    }

    public class NumHashEntity
    {
        public int NumA { get; set; } // コイン数
        public int NumB { get; set; } // 金額
        public NumHashEntity(
            int coins,
            int value
        )
        {
            NumA = coins;
            NumB = value;
        }
    }
}
