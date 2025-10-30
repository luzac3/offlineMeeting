namespace offlineMeeting.Models.Entity.Pos.SendOrderPurchaseEntity
{
    public class SendOrderPurchaseEntity
    {
        public ClassHashEntity ClassHash { get; set; }
        public NumHashEntity NumHash { get; set; }
        public CheckHashEntity CheckHash { get; set; }

        public SendOrderPurchaseEntity(
            string eventNumber,
            string orderResultId,
            string userCd,
            string purchaseResultId,
            int orderNumber,
            int orderCoins,
            int alcoholAmount,
            bool isAlcohol
        )
        {
            ClassHash = new ClassHashEntity(
                eventNumber: eventNumber,
                orderResultId: orderResultId,
                userCd: userCd,
                purchaseResultId: purchaseResultId
            );
            NumHash = new NumHashEntity(
                orderNumber: orderNumber,
                orderCoins: orderCoins,
                alcoholAmount: alcoholAmount
            );
            CheckHash = new CheckHashEntity(isAlcohol: isAlcohol);
        }
    }

    public class ClassHashEntity
    {
        public string ClassA { get; set; } // EventNumber
        public string ClassE { get; set; } // orderResultId
        public string ClassB { get; set; } // UserCd
        public string ClassC { get; set; } // ここのResultIdは、商品のId
        public ClassHashEntity(
            string eventNumber,
            string orderResultId,
            string userCd,
            string purchaseResultId
        )
        {
            ClassA = eventNumber;
            ClassE = orderResultId;
            ClassB = userCd;
            ClassC = purchaseResultId;
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
        public bool CheckA { get; set; }
        public CheckHashEntity(bool isAlcohol)
        {
            CheckA = isAlcohol;
        }
    }
}