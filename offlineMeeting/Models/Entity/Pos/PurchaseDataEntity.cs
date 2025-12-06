namespace offlineMeeting.Models.Entity.Pos
{
    public class PurchaseDataEntity
    {
        public long? ResultId {  get; set; }
        public string? Image {  get; set; }
        public string? LowImage { get; set; }
        public string? PurchaseName { get; set; }
        public string? FoodCategory { get; set; }
        public int Category { get; set; }
        public int? CoinNumber { get; set; }
        public int? LeftAmount { get; set; }
        public bool Ordered { get; set; } = false;
        public PurchaseDataEntity(
            long? resultId,
            string? image,
            string? lowImage,
            string? purchaseName,
            string? foodCategory,
            int category,
            int? coinNumber,
            int? leftAmount,
            bool ordered
        )
        {
            ResultId = resultId;
            Image = image;
            LowImage = lowImage;
            PurchaseName = purchaseName;
            FoodCategory = foodCategory;
            Category = category;
            CoinNumber = coinNumber;
            LeftAmount = leftAmount;
            Ordered = ordered;
        }
    }
}