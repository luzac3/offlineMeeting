using offlineMeeting.Models.Entity.Pos;

namespace offlineMeeting.Models.ViewModel
{
    public class PosViewModel
    {
        public List<AlcoholDataEntity>? AlcoholDataEntityList { get; set; }
        public List<FoodDataEntity>? FoodDataEntityList { get; set; }
        public List<OrderEntity>? OrderEntityList { get; set; }
        public List<OrderTableEntity>? OrderTableEntityList { get; set; }

        public PosViewModel() { }

        public void setAlcoholDataList(List<AlcoholDataEntity> alcoholDataEntityList)
        {
            AlcoholDataEntityList = alcoholDataEntityList;
        }
        public void setFoodDataList(List<FoodDataEntity> foodDataEntityList)
        {
            FoodDataEntityList = foodDataEntityList;
        }

        public void setOrderEntityList(List<OrderEntity> orderEntityList)
        {
            OrderEntityList = orderEntityList;
        }

        public void setOrderTableEntityList(List<OrderTableEntity> orderTableEntityList)
        {
            OrderTableEntityList = orderTableEntityList;
        }
    }
}