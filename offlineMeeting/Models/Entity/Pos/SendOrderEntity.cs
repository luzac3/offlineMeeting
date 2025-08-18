namespace offlineMeeting.Models.Entity.Pos
{
    public class SendOrderEntity
    {
        public ClassHashEntity ClassHash { get; set; }
        public NumHashEntity NumHash { get; set; }
        public CheckHashEntity CheckHash { get; set; }

        public SendOrderEntity(
            string eventNumber,
            string userCd,
            string resultId,
            int orderNumber,
            int orderCoins,
            int alcoholAmount
        )
        {
            ClassHash = new ClassHashEntity(
                eventNumber: eventNumber,
                userCd: userCd,
                resultId: resultId
            );
            NumHash = new NumHashEntity(
                orderNumber: orderNumber,
                orderCoins: orderCoins,
                alcoholAmount: alcoholAmount
            );
            CheckHash = new CheckHashEntity();
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
            string resultId
        )
        {
            ClassA = eventNumber;
            ClassB = userCd;
            ClassC = resultId;
        }
    }

    public class NumHashEntity
    {
        public int NumA { get; set; } // OrderNumber
        public int NumB { get; set; } // OrderNumber
        public int NumC { get; set; } // AlcoholAmount
        public NumHashEntity(
            int orderNumber,
            int orderCoins,
            int alcoholAmount
        )
        {
            NumA = orderNumber;
            NumB = orderCoins;
            NumC = alcoholAmount;
        }
    }

    public class CheckHashEntity
    {
        public bool CheckA { get; set; } // InProgress
        public CheckHashEntity()
        {
            CheckA = true;
        }
    }
}