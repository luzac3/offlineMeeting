namespace offlineMeeting.Models.Entity.Pos.SendOrderPurchaseEntity
{
    public class SendOrderPurchaseEntity
    {
        public string ClassA { get; set; } // EventNumber
        public string ClassE { get; set; } // orderResultId
        public string ClassB { get; set; } // UserCd
        public string ClassC { get; set; } // ここのResultIdは、商品のId
        public int NumA { get; set; } // OrderNumber
        public int NumB { get; set; } // OrderNumber
        public int NumC { get; set; } // AlcoholAmount
        public bool CheckA { get; set; }

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
            ClassA = eventNumber;
            ClassE = orderResultId;
            ClassB = userCd;
            ClassC = purchaseResultId;
            NumA = orderNumber;
            NumB = orderCoins;
            NumC = alcoholAmount;
            CheckA = isAlcohol;
        }
    }
}