using offlineMeeting.Models.Entity.Pos;
using offlineMeeting.Models.Entity.Share;

namespace offlineMeeting.Models.ViewModel
{
    public class CollectionViewModel
    {
        public List<AlcoholDataEntity>? AlcoholDataEntityList { get; set; }
        public AlcoholDataEntity? AlcoholDataEntity { get; set; }
        public List<OrderEntity>? OrderEntityList { get; set; }
        public List<OrderPurchaseTableEntity>? OrderPurchaseTableEntity { get; set; }

        public CollectionViewModel() { }

        public void setAlcoholDataList(List<AlcoholDataEntity> alcoholDataEntityList)
        {
            AlcoholDataEntityList = alcoholDataEntityList;
        }

        public void setAlcoholDataEntity(AlcoholDataEntity alcoholDataEntity)
        {
            AlcoholDataEntity = alcoholDataEntity;
        }

        public void setOrderEntityList(List<OrderEntity> orderEntityList)
        {
            OrderEntityList = orderEntityList;
        }

        public void setOrderPurchaseTableEntityList(List<OrderPurchaseTableEntity> orderPurchaseTableEntity)
        {
            OrderPurchaseTableEntity = orderPurchaseTableEntity;
        }
    }
}