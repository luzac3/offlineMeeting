namespace offlineMeeting.Models.Entity.Pos
{
    public class OrderPostEntity
    {
        public int ResultId { get; set; }
        public int Category { get; set; } // 0:エクストラ, 1:フード, 2:無加工品, 3:アルコール
        public int OrderNumber { get; set; }
        public int OrderCoins { get; set; }
        public int AlcoholAmount { get; set; }

        public OrderPostEntity() { }
    }
}