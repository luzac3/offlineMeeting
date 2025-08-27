using offlineMeeting.Models.DBProperty;
using offlineMeeting.Models.Entity.Pos;
using offlineMeeting.Models.Entity.Share;
using offlineMeeting.Models.Process.DBProcess;
using offlineMeeting.Models.Process.Share;
using offlineMeeting.Models.Share;
using System.Data;

namespace offlineMeeting.Models.Process.Pos
{
    public class GetUsersOrderProcess : ResultsProcess
    {
        private HttpContext HttpContext;
        private int OrderSiteId;
        private int JoinerSiteId;
        private string EventId;

        public GetUsersOrderProcess(int orderSiteId, int joinerSiteId, HttpContext httpContext)
        {
            HttpContext = httpContext;
            ManageEventId manageEventId = new ManageEventId(httpContext);

            EventId = manageEventId.GetEventId();
            if (EventId == "0")
            {
                throw new DataException("イベントIDが不正です");
            }
            OrderSiteId = orderSiteId;
            JoinerSiteId = joinerSiteId;
        }


        public List<OrderTableEntity> Get()
        {
            List<ResultsEntity> resultsEntityList = base
                .GetFromSiteId(OrderSiteId)
                .Where(x => x.ClassA == EventId)
                .Where(x => x.CheckA == true)
                .ToList();

            List<OrderTableEntity> orderTableEntity = new List<OrderTableEntity>();

            foreach (ResultsEntity resultsEntity in resultsEntityList)
            {
                GetUsersProcess getUsersProcess = new(JoinerSiteId, HttpContext);
                JoinerEntity userData = getUsersProcess.GetUserData(resultsEntity.ClassB ?? string.Empty);
                ResultsEntity? purchaseName = String.IsNullOrEmpty(resultsEntity.ClassC) ? null : base.GetSingleRecord(resultsEntity.ClassC);

                orderTableEntity
                    .Add(
                        new OrderTableEntity(
                            resultId: resultsEntity.ResultId.ToString(),
                            userCd: resultsEntity.ClassB,
                            userName: userData.UserName,
                            purchaseCd: resultsEntity.ClassC,
                            purchaseName: purchaseName?.ClassA,
                            number: Convert.ToInt32(resultsEntity.NumA),
                            unit: resultsEntity.ClassD,
                            coins: Convert.ToInt32(resultsEntity.NumB),
                            alcoholQuantity: Convert.ToInt32(resultsEntity.NumC),
                            unProvided: resultsEntity.CheckA ?? false
                        )
                    );
            }
            return orderTableEntity;
        }
    }
}