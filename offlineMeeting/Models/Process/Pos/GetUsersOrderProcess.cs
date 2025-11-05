using offlineMeeting.Models.DBProperty;
using offlineMeeting.Models.Entity.Pos;
using offlineMeeting.Models.Entity.Share;
using offlineMeeting.Models.Process.DBProcess;
using offlineMeeting.Models.Process.Share;
using offlineMeeting.Models.Share;
using System.Data;

namespace offlineMeeting.Models.Process.Pos
{
    // オーダーを取得するように変更し、必要がない場合は商品のみを取得する
    public class GetUsersOrderProcess : ResultsProcess
    {
        private HttpContext HttpContext;
        private int OrderSiteId;
        private int OrderPurchaseSiteId;
        private int JoinerSiteId;
        private string EventId;

        public GetUsersOrderProcess(int orderSiteId, int orderPurchaseSiteId, int joinerSiteId, HttpContext httpContext, string electronEid = "")
        {
            HttpContext = httpContext;

            if(!string.IsNullOrEmpty(electronEid))
            {
                EventId = electronEid;
            }
            else
            {
                ManageEventId manageEventId = new ManageEventId(httpContext);
                EventId = manageEventId.GetEventId();
            }

            if (EventId == "0")
            {
                throw new DataException("イベントIDが不正です");
            }

            OrderSiteId = orderSiteId;
            OrderPurchaseSiteId = orderPurchaseSiteId;
            JoinerSiteId = joinerSiteId;
        }

        public List<OrderTableEntity> Get()
        {
            List<ResultsEntity> resultsEntityList = base
                .GetFromSiteId(OrderSiteId)
                .Where(x => x.ClassA == EventId)
                .Where(x => x.CheckA == true)
                .OrderByDescending(x => x.ResultId)
                .ToList();

            return GetOrder(resultsEntityList);
        }

        public List<OrderTableEntity> Get(string resultId)
        {
            List<ResultsEntity> resultsEntityList = new List<ResultsEntity>()
            {
                base.GetSingleRecord(resultId)
            };

            return GetOrder(resultsEntityList);
        }

        public List<OrderTableEntity> GetOwn(HttpContext httpContext)
        {
            ManageUserCd manageUserCd = new(httpContext);
            string userCd = manageUserCd.GetUserCd();

            List<ResultsEntity> resultsEntityList = base
                .GetFromSiteId(OrderSiteId)
                .Where(x => x.ClassA == EventId)
                .Where(x => x.ClassD == userCd)
                .OrderByDescending(x => x.ResultId)
                .ToList();

            return GetOrder(resultsEntityList);
        }

        private List<OrderTableEntity> GetOrder(List<ResultsEntity> resultsEntityList)
        {

            List<OrderTableEntity> orderTableEntity = new List<OrderTableEntity>();

            foreach (ResultsEntity resultsEntity in resultsEntityList)
            {
                GetUsersProcess getUsersProcess = new(JoinerSiteId, HttpContext);
                JoinerEntity userData = getUsersProcess.GetUserData(resultsEntity.ClassD ?? string.Empty);

                string? orderNumber = resultsEntity?.ClassB?.ToString();
                string[] orderNumbers = string.IsNullOrEmpty(orderNumber) ? [] : orderNumber.Split('-');
                if (orderNumbers.Length >= 3)
                {
                    orderNumber = $"{orderNumbers[2]}";
                }

                orderTableEntity
                    .Add(
                        new OrderTableEntity(
                            resultId: resultsEntity!.ResultId.ToString(),
                            userCd: resultsEntity.ClassD,
                            userName: userData.UserName,
                            orderNumber: orderNumber,
                            coins: Convert.ToInt32(resultsEntity.NumA),
                            isAlcohol: resultsEntity.CheckB ?? false,
                            unProvided: resultsEntity.CheckA ?? false,
                            orderPurchaseTableEntityList: GetPurchase(resultsEntity.ResultId.ToString()!, userData.UserName ?? "")
                        )
                    );
            }

            return orderTableEntity;
        }

        private List<OrderPurchaseTableEntity> GetPurchase(string resultId, string userName)
        {
            // resultIdのリストで取得する
            List<ResultsEntity> resultsEntityList = base
                .GetFromSiteId(OrderPurchaseSiteId)
                .Where(x => x.ClassA == EventId)
                .Where(x => x.ClassE == resultId)
                .OrderByDescending(x=>x.ResultId)
                .ToList();

            List<OrderPurchaseTableEntity> orderPurchaseTableEntity = new List<OrderPurchaseTableEntity>();

            foreach (ResultsEntity resultsEntity in resultsEntityList)
            {
                ResultsEntity? purchaseData = String.IsNullOrEmpty(resultsEntity.ClassC) ? null : base.GetSingleRecord(resultsEntity.ClassC);

                string? purchaseName = Int32.TryParse(purchaseData?.ClassA, out int pursedPurchaseData) ?
                    base.GetSingleRecord(pursedPurchaseData.ToString()).Title :
                    purchaseData?.ClassA;

                orderPurchaseTableEntity
                    .Add(
                        new OrderPurchaseTableEntity(
                            resultId: resultsEntity.ResultId.ToString(),
                            userCd: resultsEntity.ClassB,
                            userName: userName,
                            purchaseCd: resultsEntity.ClassC,
                            purchaseName: purchaseName,
                            purchaseRead: purchaseData?.ClassJ,
                            purchaseAbbreviation: purchaseData?.ClassI,
                            number: Convert.ToInt32(resultsEntity.NumA),
                            unit: resultsEntity.ClassD,
                            coins: Convert.ToInt32(resultsEntity.NumB),
                            alcoholQuantity: Convert.ToInt32(resultsEntity.NumC),
                            isAlcohol: resultsEntity.CheckA ?? false,
                            hide: resultsEntity.CheckZ ?? false
                        )
                    );
            }
            return orderPurchaseTableEntity;
        }
    }
}