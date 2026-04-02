using offlineMeeting.Models.Entity.Share;

namespace offlineMeeting.Models.Entity.Pos.SendOrderEntity
{
    public class SendOrderEntity: ApiEntity
    {
        public string ClassA { get; set; } // EventNumber
        public string ClassD { get; set; } // UserCd
        public int NumA { get; set; } // OrderCoins
        public bool CheckA { get; set; } // InProgress
        public bool CheckB { get; set; } // IsAlcohol

        public SendOrderEntity(
            string eventNumber,
            string userCd,
            int orderCoins,
            bool isAlcohol
        )
        {
            ClassA = eventNumber;
            ClassD = userCd;
            NumA = orderCoins;
            CheckA = true;
            CheckB = isAlcohol;
        }
    }
}