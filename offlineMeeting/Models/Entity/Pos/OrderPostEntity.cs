namespace offlineMeeting.Models.Entity.Pos
{
    public class OrderPostEntity
    {
        public int ResultId { get; set; }
        public int OrderNumber { get; set; }
        public int OrderCoins { get; set; }
        public int AlcoholAmount { get; set; }

        public OrderPostEntity() { }
    }
}
