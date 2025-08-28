namespace offlineMeeting.Models.Entity.Pos
{
    public class OrderTableEntity
    {
        public string? ResultId { get; set; }
        public string? UserCd { get; set; }
        public string? UserName { get; set; }
        public string? PurchaseCd { get; set; }
        public string? PurchaseName { get; set; }
        public string? PurchaseRead { get; set; }
        public int Number { get; set; }
        public string? Unit { get; set; }
        public int Coins { get; set; }
        public int AlcoholQuantity { get; set; }
        public bool UnProvided { get; set; }
        public OrderTableEntity(
            string? resultId = null,
            string? userCd = null,
            string? userName = null,
            string? purchaseCd = null,
            string? purchaseName = null,
            string? purchaseRead = null,
            int number = 0,
            string? unit = null,
            int coins = 0,
            int alcoholQuantity = 0,
            bool unProvided = false
        )
        {
            ResultId = resultId;
            UserCd = userCd;
            UserName = userName;
            PurchaseCd = purchaseCd;
            PurchaseName = purchaseName;
            PurchaseRead = purchaseRead;
            Number = number;
            Unit = unit;
            Coins = coins;
            AlcoholQuantity = alcoholQuantity;
            UnProvided = unProvided;
        }
    }
}