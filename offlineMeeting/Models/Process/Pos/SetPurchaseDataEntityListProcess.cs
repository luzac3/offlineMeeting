using offlineMeeting.Models.DBProperty;
using offlineMeeting.Models.Entity.Pos;
using offlineMeeting.Models.Process.DBProcess;
using offlineMeeting.Models.Process.Share;

namespace offlineMeeting.Models.Process.Pos
{
    public class SetPurchaseDataEntityListProcess : ResultsProcess
    {
        private int ExtraFoodSiteId;
        private int EventFoodSiteId;
        private int RawFoodSiteId;
        private int OrderSiteId;
        private string EventId;
        private string UserCd;
        List<PurchaseDataEntity> PurchaseDataEntityList;

        public SetPurchaseDataEntityListProcess(
            int extraFoodSiteId,
            int eventFoodSiteId,
            int rawFoodSiteId,
            int orderSiteId,
            string eventId,
            string userCd
        )
        {
            ExtraFoodSiteId = extraFoodSiteId;
            EventFoodSiteId = eventFoodSiteId;
            RawFoodSiteId = rawFoodSiteId;
            OrderSiteId = orderSiteId;
            EventId = eventId;
            UserCd = userCd;
            PurchaseDataEntityList = new List<PurchaseDataEntity> { };
        }

        public List<PurchaseDataEntity> Get(int category)
        {
            List<ResultsEntity> results = base
                .GetFromSiteIdList(
                    new List<int> { ExtraFoodSiteId, EventFoodSiteId, RawFoodSiteId }
                )
                .Where(x=>x.ClassD == EventId)
                .ToList();

            GetPurchaseDataEntityList(results, category);

            return PurchaseDataEntityList;
        }

        public List<PurchaseDataEntity> Get(List<int> resultId)
        {
            List<ResultsEntity> results = base
                .GetFromSiteId(EventFoodSiteId, resultId)
                .Where(x => x.ClassD == EventId)
                .ToList();

            GetPurchaseDataEntityList(results);

            return PurchaseDataEntityList;
        }

        private void GetPurchaseDataEntityList(List<ResultsEntity> results, int category = 0)
        {
            List<int> orderedResultsIdList = base
                .GetFromSiteId(OrderSiteId)
                .Where(x => x.ClassA == EventId)
                .Where(x => x.ClassB == UserCd)
                .Select(x => Convert.ToInt32(x.ClassC ?? "0"))
                .ToList();

            foreach (ResultsEntity result in results)
            {
                string purchaseName = "";
                if (!string.IsNullOrEmpty(result.ClassA) && category == 1)
                {
                    ResultsEntity? purchaseEntity = base.GetSingleRecord(result.ClassA);
                    purchaseName = purchaseEntity?.Title ?? "";
                }

                string[]? imageSplit = result!.Body?.Split("/");
                string guid = "";
                if (imageSplit != null && imageSplit?.Length > 2)
                {
                    guid = string.IsNullOrEmpty(imageSplit?[2]) ? "" : imageSplit[2];
                }

                GetBinariesProcess getBinariesProcess = new GetBinariesProcess();
                string? image = getBinariesProcess.GetImage(guid);
                PurchaseDataEntityList
                    .Add(
                        new PurchaseDataEntity(
                            resultId: result.ResultId,
                            image: image == null ? "" : $"data:image/jpeg;base64,{image}",
                            purchaseName: purchaseName,
                            category: category,
                            coinNumber: Convert.ToInt32(result.NumG),
                            leftAmount: Convert.ToInt32(result.NumE),
                            ordered: orderedResultsIdList.Contains(Convert.ToInt32(result.ResultId))
                        )
                    );
            }
        }
    }
}
