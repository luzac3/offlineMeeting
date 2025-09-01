namespace offlineMeeting.Models.Entity.Pos
{
    public class FoodDataEntity
    {
        public long? ResultId {  get; set; }
        public string? Image {  get; set; }
        public string? FoodName { get; set; }
        public string? CookingCategory { get; set; }
        public int? CoinNumber { get; set; }
        public int? LeftAmount { get; set; }
        public bool Ordered { get; set; } = false;
        public FoodDataEntity(
            long? resultId,
            string? image,
            string? foodName,
            string? cookingCategory,
            int? coinNumber,
            int? leftAmount,
            bool ordered
        )
        {
            ResultId = resultId;
            Image = image;
            FoodName = foodName;
            CookingCategory = cookingCategory;
            CoinNumber = coinNumber;
            LeftAmount = leftAmount;
            Ordered = ordered;
        }
    }
}