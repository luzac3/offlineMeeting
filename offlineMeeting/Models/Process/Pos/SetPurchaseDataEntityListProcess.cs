using offlineMeeting.Models.Entity.Pos;
using offlineMeeting.Models.Process.Share;
using PleasanterBridge.src.DataRepository.Entity;
using PleasanterBridge.src.DataRepository.Service;
using System.Text.Json;

namespace offlineMeeting.Models.Process.Pos
{
    public class SetPurchaseDataEntityListProcess
    {
        private long ExtraFoodSiteId;
        private long EventFoodSiteId;
        private long RawFoodSiteId;
        private long OrderSiteId;
        private long EventId;
        private long UserCd;
        List<PurchaseDataEntity> PurchaseDataEntityList;
        private readonly IPleasanterRepository _pleasanterRepository;

        public SetPurchaseDataEntityListProcess(
            long extraFoodSiteId,
            long eventFoodSiteId,
            long rawFoodSiteId,
            long orderSiteId,
            long eventId,
            long userCd,
            IPleasanterRepository pleasanterRepository
        )
        {
            ExtraFoodSiteId = extraFoodSiteId;
            EventFoodSiteId = eventFoodSiteId;
            RawFoodSiteId = rawFoodSiteId;
            OrderSiteId = orderSiteId;
            EventId = eventId;
            UserCd = userCd;
            PurchaseDataEntityList = new List<PurchaseDataEntity> { };
            _pleasanterRepository = pleasanterRepository;
        }

        public List<PurchaseDataEntity> Get(int category)
        {
            List<TablesEntity> results = category switch
            {
                0 => _pleasanterRepository.Select(ExtraFoodSiteId, null, true),
                1 => _pleasanterRepository.Select(EventFoodSiteId, null, true)
                    .Concat(_pleasanterRepository.Select(RawFoodSiteId, null, true))
                    .ToList(),
                _ => new List<TablesEntity> { }
            };

            results = FilterByEventId(results);

            GetPurchaseDataEntityList(results, _ => category);

            return PurchaseDataEntityList;
        }

        public List<PurchaseDataEntity> Get(List<long> resultId, Dictionary<string, int> categoryDic)
        {
            List<TablesEntity> results = FilterByEventId(
                _pleasanterRepository.Select(resultId, true)
            );

            GetPurchaseDataEntityList(results, result => categoryDic[$"{result.ReferenceId.ToString()}_{Convert.ToInt32(result.NumE).ToString()}"]);

            return PurchaseDataEntityList;
        }

        private List<TablesEntity> FilterByEventId(List<TablesEntity> tablesEntities)
        {
            return tablesEntities
                .Where(x =>
                    !string.IsNullOrEmpty(x.ClassD) &&
                    JsonSerializer.Deserialize<string[]>(x.ClassD)?.Contains(EventId.ToString()) == true
                )
                .ToList();
        }

        private void GetPurchaseDataEntityList(List<TablesEntity> tablesEntities, Func<TablesEntity, int> getCategory)
        {
            var where = new Dictionary<string, object>
            {
                { "ClassA", EventId.ToString() },
                { "ClassB", UserCd.ToString() }
            };

            List<int> orderedResultsIdList = _pleasanterRepository.Select(OrderSiteId, where, true)
                .Select(x => Convert.ToInt32(x.ClassC ?? "0"))
                .ToList();

            tablesEntities.Select(tablesEntity =>
            {
                if (tablesEntity == null)
                {
                    return null;
                }

                string? purchaseName = Int32.TryParse(tablesEntity?.ClassA, out int pursedPurchaseData) ?
                    _pleasanterRepository.Select(pursedPurchaseData, true).FirstOrDefault()?.Title :
                    tablesEntity?.ClassA;
                string? image = tablesEntity?.Body?[0];

                string imageBase64 = "";
                string lowImageBase64 = "";
                int width = 1475;
                int height = 1258;

                if (!string.IsNullOrEmpty(image))
                {
                    try
                    {
                        using ImageHelper imageHelper = new ImageHelper(Convert.FromBase64String(image));
                        (width, height) = imageHelper.GetSize();
                        imageBase64 = $"data:image/jpeg;base64,{image}";
                        lowImageBase64 = $"data:image/webp;base64,{imageHelper.GetLowQualityBase64()}";
                    }
                    catch
                    {
                        // fixme エラー処理
                    }
                }

                return new PurchaseDataEntity(
                    resultId: tablesEntity!.ReferenceId,
                    image: imageBase64,
                    lowImage: lowImageBase64,
                    width: width,
                    height: height,
                    purchaseName: purchaseName,
                    foodCategory: tablesEntity!.ClassB,
                    category: getCategory(tablesEntity!),
                    coinNumber: Convert.ToInt32(tablesEntity!.NumG),
                    leftAmount: Convert.ToInt32(tablesEntity!.NumH),
                    ordered: orderedResultsIdList.Contains(Convert.ToInt32(tablesEntity!.ReferenceId))
                );
            });
        }
    }
}