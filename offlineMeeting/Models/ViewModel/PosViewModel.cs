using offlineMeeting.Models.Entity.Pos;

namespace offlineMeeting.Models.ViewModel
{
    public class PosViewModel
    {
        //public List<DBEntity.EventDataEntity>? EventDataEntityList { get; set; }
        public List<AlcoholDataEntity>? AlcoholDataEntityList { get; set; }

        public PosViewModel() { }
        /*
        public void setPosDataEntityList(List<DBEntity.EventDataEntity> eventDataEntityList)
        {
            EventDataEntityList = eventDataEntityList;
        }
        */
        public void setAlcoholDataList(List<AlcoholDataEntity> alcoholDataEntityList)
        {
            AlcoholDataEntityList = alcoholDataEntityList;
        }
    }
}