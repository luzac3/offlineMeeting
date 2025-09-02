using offlineMeeting.Models.Entity.Share;
namespace offlineMeeting.Models.Entity.Pos
{
    public class OrderEntity
    {
        public OrderPostEntity OrderPostEntity { get; set; }
        public AlcoholDataEntity? AlcoholDataEntity { get; set; }
        public PurchaseDataEntity? PurchaseDataEntity { get; set; }

        public OrderEntity(
            OrderPostEntity orderPostEntity,
            AlcoholDataEntity? alcoholDataEntity = null,
            PurchaseDataEntity? purchaseDataEntity = null
        )
        {
            OrderPostEntity = orderPostEntity;
            AlcoholDataEntity = alcoholDataEntity;
            PurchaseDataEntity = purchaseDataEntity;
        }
    }
}