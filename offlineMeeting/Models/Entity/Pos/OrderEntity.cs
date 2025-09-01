namespace offlineMeeting.Models.Entity.Pos
{
    public class OrderEntity
    {
        public OrderPostEntity OrderPostEntity { get; set; }
        public AlcoholDataEntity? AlcoholDataEntity { get; set; }
        public FoodDataEntity? FoodDataEntity { get; set; }

        public OrderEntity(
            OrderPostEntity orderPostEntity,
            AlcoholDataEntity? alcoholDataEntity = null,
            FoodDataEntity? foodDataEntity = null
        )
        {
            OrderPostEntity = orderPostEntity;
            AlcoholDataEntity = alcoholDataEntity;
            FoodDataEntity = foodDataEntity;
        }
    }
}