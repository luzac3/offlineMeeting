using Microsoft.AspNetCore.Http;
using offlineMeeting.Models.DBProperty;
using offlineMeeting.Models.Entity.Pos;
using offlineMeeting.Models.Entity.Share;
using offlineMeeting.Models.JsonDataProperty;
using offlineMeeting.Models.Process.DBProcess;
using offlineMeeting.Models.Process.Share;
using offlineMeeting.Models.Share;
using System.Data;
using System.Net.Http;

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
                .OrderByDescending(x=>x.ResultId)
                .ToList();

            List<OrderTableEntity> orderTableEntity = new List<OrderTableEntity>();

            foreach (ResultsEntity resultsEntity in resultsEntityList)
            {
                GetUsersProcess getUsersProcess = new(JoinerSiteId, HttpContext);
                JoinerEntity userData = getUsersProcess.GetUserData(resultsEntity.ClassB ?? string.Empty);
                ResultsEntity? purchaseData = String.IsNullOrEmpty(resultsEntity.ClassC) ? null : base.GetSingleRecord(resultsEntity.ClassC);


                string? purchaseName = Int32.TryParse(purchaseData?.ClassA, out int pursedPurchaseData) ?
                    base.GetSingleRecord(pursedPurchaseData.ToString()).Title :
                    purchaseData?.ClassA;

                orderTableEntity
                    .Add(
                        new OrderTableEntity(
                            resultId: resultsEntity.ResultId.ToString(),
                            userCd: resultsEntity.ClassB,
                            userName: userData.UserName,
                            purchaseCd: resultsEntity.ClassC,
                            purchaseName: purchaseName,
                            purchaseRead: purchaseData?.ClassJ,
                            number: Convert.ToInt32(resultsEntity.NumA),
                            unit: resultsEntity.ClassD,
                            coins: Convert.ToInt32(resultsEntity.NumB),
                            alcoholQuantity: Convert.ToInt32(resultsEntity.NumC),
                            unProvided: resultsEntity.CheckA ?? false,
                            hide: resultsEntity.CheckZ ?? false
                        )
                    );
            }
            return orderTableEntity;
        }

        public List<OrderTableEntity> GetOwn(HttpContext httpContext)
        {
            List<ResultsEntity> resultsEntityList = base
                .GetFromSiteId(OrderSiteId)
                .Where(x => x.ClassA == EventId)
                .OrderByDescending(x => x.ResultId)
                .ToList();

            List<OrderTableEntity> orderTableEntity = new List<OrderTableEntity>();

            foreach (ResultsEntity resultsEntity in resultsEntityList)
            {
                GetUsersProcess getUsersProcess = new(JoinerSiteId, HttpContext);
                JoinerEntity userData = getUsersProcess.GetUserData(resultsEntity.ClassB ?? string.Empty);
                ResultsEntity? purchaseData = String.IsNullOrEmpty(resultsEntity.ClassC) ? null : base.GetSingleRecord(resultsEntity.ClassC);

                string? purchaseName = Int32.TryParse(purchaseData?.ClassA, out int pursedPurchaseData) ?
                    base.GetSingleRecord(pursedPurchaseData.ToString()).Title :
                    purchaseData?.ClassA;

                orderTableEntity
                    .Add(
                        new OrderTableEntity(
                            resultId: resultsEntity.ResultId.ToString(),
                            userCd: resultsEntity.ClassB,
                            userName: userData.UserName,
                            purchaseCd: resultsEntity.ClassC,
                            purchaseName: purchaseName,
                            purchaseRead: "",
                            number: Convert.ToInt32(resultsEntity.NumA),
                            unit: resultsEntity.ClassD,
                            coins: Convert.ToInt32(resultsEntity.NumB),
                            alcoholQuantity: Convert.ToInt32(resultsEntity.NumC),
                            unProvided: resultsEntity.CheckA ?? false,
                            hide: resultsEntity.CheckZ ?? false
                        )
                    );
            }

            ManageUserCd manageUserCd = new(httpContext);
            string userCd = manageUserCd.GetUserCd();

            return orderTableEntity.Where(x => x.UserCd == userCd).ToList();
        }
    }
}