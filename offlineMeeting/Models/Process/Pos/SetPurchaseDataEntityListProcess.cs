using Microsoft.AspNetCore.Hosting;
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
        private readonly IWebHostEnvironment? _env;
        private readonly string? _sessionId;

        public SetPurchaseDataEntityListProcess(
            long extraFoodSiteId,
            long eventFoodSiteId,
            long rawFoodSiteId,
            long orderSiteId,
            long eventId,
            long userCd,
            IPleasanterRepository pleasanterRepository,
            IWebHostEnvironment? env = null
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
            _env = env;
            _sessionId = pleasanterRepository.Select(eventId, true).FirstOrDefault()?.ClassB;
        }

        public List<PurchaseDataEntity> Get(int category)
        {
            var where = new Dictionary<string, object>
            {
                { "ClassD", new string[] { EventId.ToString() } }
            };

            List<TablesEntity> results = category switch
            {
                0 => _pleasanterRepository.Select(ExtraFoodSiteId, where, true),
                1 => _pleasanterRepository.Select(EventFoodSiteId, where, true)
                    .Concat(_pleasanterRepository.Select(RawFoodSiteId, where, true))
                    .ToList(),
                _ => new List<TablesEntity> { }
            };

            GetPurchaseDataEntityList(results, _ => category);

            return PurchaseDataEntityList;
        }

        public List<PurchaseDataEntity> Get(List<long> resultIds, Dictionary<string, int> categoryDic)
        {
            List<TablesEntity> results = _pleasanterRepository.Select(resultIds, true)
                .Where(x =>
                    !string.IsNullOrEmpty(x.ClassD) &&
                    JsonSerializer.Deserialize<string[]>(x.ClassD)?.Contains(EventId.ToString()) == true
                )
                .ToList(); ;

            GetPurchaseDataEntityList(results, result => categoryDic[$"{result.ReferenceId.ToString()}_{Convert.ToInt32(result.NumE).ToString()}"]);

            return PurchaseDataEntityList;
        }

        private void GetPurchaseDataEntityList(List<TablesEntity> tablesEntities, Func<TablesEntity, int> getCategory)
        {
            var where = new Dictionary<string, object>
            {
                { "ClassA", EventId.ToString() },
                { "ClassD", UserCd.ToString() }
            };

            List<int> orderedResultsIdList = _pleasanterRepository.Select(OrderSiteId, where, true)
                .Select(x => Convert.ToInt32(x.ClassC ?? "0"))
                .ToList();

            tablesEntities.ForEach(tablesEntity =>
            {
                if (tablesEntity == null)
                {
                    return;
                }

                string? purchaseName = Int64.TryParse(tablesEntity?.ClassA, out long pursedPurchaseData) ?
                    _pleasanterRepository.Select(pursedPurchaseData, true).FirstOrDefault()?.Title :
                    tablesEntity?.ClassA;
                string? image = tablesEntity?.Body?[0];

                string imageBase64 = "";
                string lowImageBase64 = "";
                int width = 1475;
                int height = 1258;

                // SessionIDフォルダに展開済みの画像があれば、そのURLパスを優先利用する
                string? folderImagePath = _env != null && tablesEntity?.SiteId != null
                    ? EventImageResolver.ResolveImageWebPath(_env, _sessionId, tablesEntity.SiteId, tablesEntity.ReferenceId, 0)
                    : null;

                if (folderImagePath != null)
                {
                    imageBase64 = folderImagePath;
                    lowImageBase64 = folderImagePath;
                    if (!string.IsNullOrEmpty(image))
                    {
                        try
                        {
                            using ImageHelper imageHelper = new ImageHelper(Convert.FromBase64String(image));
                            (width, height) = imageHelper.GetSize();
                        }
                        catch
                        {
                            // fixme エラー処理
                        }
                    }
                }
                else if (!string.IsNullOrEmpty(image))
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

                PurchaseDataEntityList.Add(new PurchaseDataEntity(
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
                ));
            });
        }
    }
}