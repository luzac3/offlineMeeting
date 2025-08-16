using offlineMeeting.Models.Entity.Pos;

namespace offlineMeeting.Models.ViewModel
{
    public class PosViewModel
    {
        public List<AlcoholDataEntity>? AlcoholDataEntityList { get; set; }

        public PosViewModel() { }

        public void setAlcoholDataList(List<AlcoholDataEntity> alcoholDataEntityList)
        {
            AlcoholDataEntityList = alcoholDataEntityList;
        }
    }
}