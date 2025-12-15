using offlineMeeting.Models.DBProperty;
using offlineMeeting.Models.Entity.Pos;
using offlineMeeting.Models.Entity.Share;
using offlineMeeting.Models.JsonDataProperty;
using offlineMeeting.Models.Process.DBProcess;
using offlineMeeting.Models.Process.Share;
using offlineMeeting.Models.Share;
using PleasanterBridge.src.DataRepository.Entity;
using PleasanterBridge.src.DataRepository.Service;
using System.Data;

namespace offlineMeeting.Models.Process.Pos
{
    // オーダーを取得するように変更し、必要がない場合は商品のみを取得する
    public class GetUsersOrderProcess
    {
        private HttpContext HttpContext;
        private readonly IPleasanterRepository _pleasanterRepository;
        private long OrderSiteId;
        private long OrderPurchaseSiteId;
        private long JoinerSiteId;
        private long EventId;

        public GetUsersOrderProcess(
            long orderSiteId,
            long orderPurchaseSiteId,
            long joinerSiteId, 
            HttpContext httpContext, 
            IPleasanterRepository pleasanterRepository,
            long electronEid = 0
        )
        {
            HttpContext = httpContext;
            _pleasanterRepository = pleasanterRepository;

            if (electronEid != 0)
            {
                EventId = electronEid;
            }
            else
            {
                ManageEventId manageEventId = new ManageEventId(httpContext);
                EventId = manageEventId.GetEventId();
            }

            OrderSiteId = orderSiteId;
            OrderPurchaseSiteId = orderPurchaseSiteId;
            JoinerSiteId = joinerSiteId;
        }

        public List<OrderTableEntity> Get()
        {
            var where = new Dictionary<string, object>
            {
                { "ClassA", EventId },
                { "CheckA", true }
            };

            List<TablesEntity> tablesEntities = _pleasanterRepository.Select(JoinerSiteId, where, true)
                .OrderByDescending(x => x.ReferenceId)
                .ToList();


            return GetOrder(tablesEntities);
        }

        public List<OrderTableEntity> Get(long resultId)
        {
            List<TablesEntity> tablesEntities = _pleasanterRepository.Select(resultId, true);

            return GetOrder(tablesEntities);
        }

        public List<OrderTableEntity> GetOwn(HttpContext httpContext)
        {
            ManageUserCd manageUserCd = new(httpContext);
            long userCd = manageUserCd.GetUserCd();
            var where = new Dictionary<string, object>
            {
                { "ClassA", EventId },
                { "ClassD", userCd.ToString() }
            };

            List<TablesEntity> resultsEntityList = _pleasanterRepository.Select(OrderSiteId, where, true)
                .OrderByDescending(x => x.ReferenceId)
                .ToList();

            return GetOrder(resultsEntityList);
        }

        private List<OrderTableEntity> GetOrder(List<TablesEntity> tablesEntities)
        {

            List<OrderTableEntity> orderTableEntity = new List<OrderTableEntity>();

            foreach (TablesEntity tablesEntity in tablesEntities)
            {
                GetUsersProcess getUsersProcess = new(HttpContext, _pleasanterRepository, JoinerSiteId);
                int.TryParse(tablesEntity.ClassD, out int userCd);
                JoinerEntity userData = getUsersProcess.GetUserData(userCd);

                string? orderNumber = tablesEntity?.ClassB?.ToString();
                string[] orderNumbers = string.IsNullOrEmpty(orderNumber) ? [] : orderNumber.Split('-');
                if (orderNumbers.Length >= 3)
                {
                    orderNumber = $"{orderNumbers[2]}";
                }

                orderTableEntity
                    .Add(
                        new OrderTableEntity(
                            resultId: tablesEntity!.ReferenceId.ToString(),
                            userCd: tablesEntity.ClassD,
                            userName: userData.UserName,
                            orderNumber: orderNumber,
                            coins: Convert.ToInt32(tablesEntity.NumA),
                            isAlcohol: tablesEntity.CheckB ?? false,
                            unProvided: tablesEntity.CheckA ?? false,
                            orderPurchaseTableEntityList: GetPurchase(tablesEntity.ReferenceId.ToString()!, userData.UserName ?? "")
                        )
                    );
            }

            return orderTableEntity;
        }

        private List<OrderPurchaseTableEntity> GetPurchase(string resultId, string userName)
        {
            var where = new Dictionary<string, object>
            {
                { "ClassA", EventId },
                { "ClassE", resultId }
            };

            // resultIdのリストで取得する
            List<TablesEntity> tablesEntities = _pleasanterRepository.Select(OrderPurchaseSiteId, where, true)
                .OrderByDescending(x=>x.ReferenceId)
                .ToList();

            List<OrderPurchaseTableEntity> orderPurchaseTableEntity = (List<OrderPurchaseTableEntity>)tablesEntities.Select(tablesEntity => {
                TablesEntity? purchaseData = int.TryParse(tablesEntity.ClassC, out int userCd) ?
                    null :
                    _pleasanterRepository.Select(userCd, true).FirstOrDefault();

                string? purchaseName = Int32.TryParse(purchaseData?.ClassA, out int pursedPurchaseData) ?
                    _pleasanterRepository.Select(pursedPurchaseData, true).FirstOrDefault()?.Title :
                    purchaseData?.ClassA;

                return new OrderPurchaseTableEntity(
                            resultId: tablesEntity.ReferenceId.ToString(),
                            userCd: tablesEntity.ClassB,
                            userName: userName,
                            purchaseCd: tablesEntity.ClassC,
                            purchaseName: purchaseName,
                            purchaseRead: purchaseData?.ClassJ,
                            purchaseAbbreviation: purchaseData?.ClassI,
                            number: Convert.ToInt32(tablesEntity.NumA),
                            unit: tablesEntity.ClassD,
                            coins: Convert.ToInt32(tablesEntity.NumB),
                            alcoholQuantity: Convert.ToInt32(tablesEntity.NumC),
                            isAlcohol: tablesEntity.CheckA ?? false,
                            hide: tablesEntity.CheckZ ?? false
                        );
            });

            return orderPurchaseTableEntity;
        }
    }
}