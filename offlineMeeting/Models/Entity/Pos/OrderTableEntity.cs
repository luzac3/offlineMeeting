namespace offlineMeeting.Models.Entity.Pos
{
    public class OrderTableEntity
    {
        public string? ResultId { get; set; }
        public string? UserCd { get; set; }
        public string? UserName { get; set; }
        public string? OrderNumber { get; set; }
        public int Coins { get; set; }
        public bool IsAlcohol { get; set; }
        public bool UnProvided { get; set; }
        public List<OrderPurchaseTableEntity>? OrderPurchaseTableEntityList { get; set; }
        public OrderTableEntity(
            string? resultId = null,
            string? userCd = null,
            string? userName = null,
            string? orderNumber = null,
            int coins = 0,
            bool isAlcohol = false,
            bool unProvided = false,
            List<OrderPurchaseTableEntity>? orderPurchaseTableEntityList = null
        )
        {
            ResultId = resultId;
            UserCd = userCd;
            UserName = userName;
            OrderNumber = orderNumber;
            Coins = coins;
            IsAlcohol = isAlcohol;
            UnProvided = unProvided;
            OrderPurchaseTableEntityList = orderPurchaseTableEntityList;
        }
    }
}