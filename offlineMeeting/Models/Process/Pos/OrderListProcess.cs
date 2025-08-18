using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using offlineMeeting.Models.Entity.Pos;
using offlineMeeting.Models.Share;
using System.Data;
using System.Net.Http;

namespace offlineMeeting.Models.Process.Pos
{
    public class OrderListProcess
    {
        private int SakeSiteId;
        private string EventId;
        private List<OrderPostEntity> OrderPostEntityList;

        public OrderListProcess(List<OrderPostEntity> orderPostEntityList, int sakeSiteId, HttpContext httpContext)
        {
            ManageEventId manageEventId = new ManageEventId(httpContext);

            EventId = manageEventId.GetEventId();
            if (EventId == "0")
            {
                throw new DataException("イベントIDが不正です");
            }
            SakeSiteId = sakeSiteId;
            OrderPostEntityList = orderPostEntityList.OrderBy(x => x.ResultId).ToList();
        }

        public List<OrderEntity> Get(System.Text.Json.Nodes.JsonNode? json)
        {
            List<OrderEntity> orderEntityList = new List<OrderEntity>();  

            List<int> resultIdList = OrderPostEntityList
                .Select(x => x.ResultId)
                .ToList();
            SetAlcoholDataEntityListProcess setAlcoholDataEntityListProcess = new SetAlcoholDataEntityListProcess(SakeSiteId, EventId);
            List<AlcoholDataEntity> alCoholDataEntityList = setAlcoholDataEntityListProcess.Get(json, resultIdList);

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