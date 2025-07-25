using offlineMeeting.Models.DBEntity;
using offlineMeeting.Models.Entity.GameData;
using offlineMeeting.Models.JsonDataProperty;

namespace offlineMeeting.Models.ViewModel
{
    public class GameDataViewModel
    {
        public DirectionProperty DirectionProperty { get; set; }
        public EndHandPostEntity EndHandPostEntity { get; set; }
        public GameEntity? GameEntity { get; set; }
        public EndHandRegisterPostEntity? EndHandRegisterPostEntity { get; set; }
        public List<GameUsersDataEntity> GameUsersDataEntityList {  get; set; }
        public List<EndHandResultEntity>? EndHandResultEntityList { get; set; }
        public List<RetVideoEntity>? RetVideoEntityList { get; set; }
        public GameDataViewModel()
        {
            DirectionProperty = new DirectionProperty();
            EndHandPostEntity = new EndHandPostEntity();
            GameUsersDataEntityList = new List<GameUsersDataEntity>();
        }

        public void setDirectionProperty(DirectionProperty directionProperty)
        {
            DirectionProperty = directionProperty;
        }

        public void setEndHandPostEntity(EndHandPostEntity endHandPostEntity)
        {
            EndHandPostEntity = endHandPostEntity;
        }
        public void setGameEntity(GameEntity gameEntity)
        {
            GameEntity = gameEntity;
        }
        public void setEndHandRegisterPostEntity(EndHandRegisterPostEntity endHandRegisterPostEntity)
        {
            EndHandRegisterPostEntity = endHandRegisterPostEntity;
        }
        public void setGameUsersDataEntityList(List<GameUsersDataEntity> gameUsersDataEntityList)
        {
            GameUsersDataEntityList = gameUsersDataEntityList;
        }
        public void setEndHandResultList(List<EndHandResultEntity> endHandResultEntityList)
        {
            EndHandResultEntityList = endHandResultEntityList;
        }

        public void setRetVideoEntityList(List<RetVideoEntity> retVideoEntityList)
        {
            RetVideoEntityList = retVideoEntityList;
        }
    }
}