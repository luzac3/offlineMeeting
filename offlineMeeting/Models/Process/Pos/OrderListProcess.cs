using offlineMeeting.Models.Entity.Pos;

namespace offlineMeeting.Models.Process.Pos
{
    public class OrderListProcess
    {
        private int SakeSiteId;
        private List<OrderPostEntity> OrderPostEntityList;

        public OrderListProcess(List<OrderPostEntity> orderPostEntityList, int sakeSiteId) {
            SakeSiteId = sakeSiteId;
            OrderPostEntityList = orderPostEntityList.OrderBy(x => x.ResultId).ToList();
        }

        public List<OrderEntity> Get(System.Text.Json.Nodes.JsonNode? json, string eventId)
        {
            List<OrderEntity> orderEntityList = new List<OrderEntity>();  

            List<int> resultIdList = OrderPostEntityList
                .Select(x => x.ResultId)
                .ToList();
            SetAlcoholDataEntityListProcess setAlcoholDataEntityListProcess = new SetAlcoholDataEntityListProcess(SakeSiteId);
            List<AlcoholDataEntity> alCoholDataEntityList = setAlcoholDataEntityListProcess.Get(json, eventId, resultIdList);

            foreach (OrderPostEntity orderPostEntity in OrderPostEntityList)
            {
                orderEntityList.Add(
                    new OrderEntity
                    (
                        orderPostEntity: orderPostEntity,
                        alcoholDataEntity: alCoholDataEntityList.Where(x => x.ResultId == orderPostEntity.ResultId).FirstOrDefault()
                    )
                );
            }

            return orderEntityList;
        }
    }
}