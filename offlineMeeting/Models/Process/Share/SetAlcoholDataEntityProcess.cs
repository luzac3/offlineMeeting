using offlineMeeting.Models.Entity.Share;
using PleasanterBridge.src.DataRepository.Entity;
using PleasanterBridge.src.DataRepository.Service;

namespace offlineMeeting.Models.Process.Share
{
    public class SetAlcoholDataEntityProcess
    {
        private long SakeSiteId;
        private long OrderSiteId;
        private long EventId;
        private long UserCd;
        List<AlcoholDataEntity> AlcoholDataEntityList;
        private readonly IPleasanterRepository _repository;

        public SetAlcoholDataEntityProcess(IPleasanterRepository repository, long sakeSiteId, long orderSiteId, long eventId, long userCd)
        {
            SakeSiteId = sakeSiteId;
            OrderSiteId = orderSiteId;
            EventId = eventId;
            UserCd = userCd;
            AlcoholDataEntityList = new List<AlcoholDataEntity> { };
            _repository = repository;
        }

        public List<AlcoholDataEntity> Get(System.Text.Json.Nodes.JsonNode? json)
        {
            var where = new Dictionary<string, object>
            {
                { "ClassH", new string[] {EventId.ToString()} }
            };

            Dictionary<string, string> choicesTextDic = GetChoicesTextDic(json);

            List<TablesEntity> results = _repository.Select(SakeSiteId, where, true);

            GetAlcoholDataEntityList(results, choicesTextDic);

            return AlcoholDataEntityList;
        }

        public List<AlcoholDataEntity> Get(System.Text.Json.Nodes.JsonNode? json, List<long> resultIds)
        {
            Dictionary<string, string> choicesTextDic = GetChoicesTextDic(json);

            List<TablesEntity> results = _repository.Select(resultIds, true);

            GetAlcoholDataEntityList(results, choicesTextDic);

            return AlcoholDataEntityList;
        }

        public AlcoholDataEntity Get(System.Text.Json.Nodes.JsonNode? json, long resultId)
        {
            Dictionary<string, string> choicesTextDic = GetChoicesTextDic(json);

            TablesEntity? result = _repository.Select(resultId, true).FirstOrDefault();

            if (result == null)
            {
                throw new InvalidOperationException("指定された resultId に該当するデータが見つかりません。");
            }

            return GetAlcoholDataEntity(result, choicesTextDic);
        }

        public List<long> GetHidedIdList()
        {
            var where = new Dictionary<string, object>
            {
                { "CheckZ", true }
            };
            
            return _repository.Select(SakeSiteId, where, true)
                .Select(x => x.ReferenceId)
                .ToList();
        }

        private Dictionary<string, string> GetChoicesTextDic(System.Text.Json.Nodes.JsonNode? json)
        {
            var columns = json?["Columns"];

            // 特定名称選択肢
            var choicesTexts = columns?.AsArray()
                .Where(x => x?["ColumnName"]?.GetValue<string>() == "ClassE")
                .Select(s => s?["ChoicesText"]?.GetValue<string>())
                .FirstOrDefault()?
                .Split(new string[] { "\n" }, StringSplitOptions.None);

            Dictionary<string, string> choicesTextDic = new Dictionary<string, string>();

            if (choicesTexts != null)
            {
                foreach (string choicesText in choicesTexts)
                {
                    string[] raw = choicesText.Split(',');
                    choicesTextDic.Add(raw[0], raw.Length < 2 ? "" : raw[1]);
                }
            }

            return choicesTextDic;
        }

        private List<long> GetOrderedResultsIdList()
        {
            var where = new Dictionary<string, object>
            {
                { "ClassA", EventId.ToString() },
                { "ClassD", UserCd.ToString() }
            };

            return _repository.Select(OrderSiteId, where, true)
                .Select(x => Convert.ToInt64(x.ClassC ?? "0"))
                .ToList();
        }

        private void GetAlcoholDataEntityList(List<TablesEntity> results, Dictionary<string, string> choicesTextDic)
        {
            List<long> orderedResultsIdList = GetOrderedResultsIdList();

            foreach (TablesEntity result in results)
            {
                AlcoholDataEntityList.Add(GetAlcoholDataEntity(result, choicesTextDic, orderedResultsIdList));
            }
        }

        private AlcoholDataEntity GetAlcoholDataEntity(TablesEntity result, Dictionary<string, string> choicesTextDic, List<long>? orderedResultsIdList = null)
        {
            orderedResultsIdList ??= GetOrderedResultsIdList();

            long breweryId = string.IsNullOrEmpty(result.ClassC) ? 0 : Convert.ToInt64(result.ClassC);
            TablesEntity? brewery = breweryId > 0 ? _repository.Select(breweryId, true).FirstOrDefault() : null;

            long todohukenId = string.IsNullOrEmpty(brewery?.ClassB) ? 0 : Convert.ToInt64(brewery.ClassB);
            string? prefectures = todohukenId > 0 ? _repository.Select(todohukenId, true).FirstOrDefault()?.ClassA : null;

            string? image = null;
            if (result?.DescriptionG != null && result.DescriptionG.Count > 0)
            {
                image = result.DescriptionG[0];
            }

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

            return new AlcoholDataEntity(
                resultId: result!.ReferenceId,
                image: imageBase64,
                lowImage: lowImageBase64,
                width: width,
                height: height,
                alcoholName: result.ClassA,
                abreviation: result.ClassI,
                brewery: brewery?.Title,
                prefectures: prefectures,
                genryoMai: result.ClassD,
                speciallyDesignated: string.IsNullOrEmpty(result.ClassE) ? "" : choicesTextDic[result.ClassE], // 特定名称
                ricePolishingRatio: Convert.ToInt32(result.NumB),
                sakeMeterValue: result.NumC,
                acidRate: result.NumG,
                abv: result.NumD,
                rarity: Convert.ToInt32(result.NumF),
                coinNumber: Convert.ToInt32(result.NumI),
                leftAmount: Convert.ToInt32(result.NumH),
                priority: result.CheckA ?? false,
                hide: result.CheckB ?? false,
                ordered: orderedResultsIdList.Contains(Convert.ToInt32(result.ReferenceId)),
                taseting: result.DescriptionE?[0].Replace("\n", "<br>"),
                rarityReason: result.DescriptionF?[0].Replace("\n", "<br>"),
                explanation: result.DescriptionC?[0].Replace("\n", "<br>"),
                mainLabel: brewery?.DescriptionA?[0].Replace("\n", "<br>"),
                breweryFeatures: brewery?.DescriptionB?[0].Replace("\n", "<br>"),
                postCode: Convert.ToInt32(brewery?.NumA),
                municipality: brewery?.ClassC,
                address: brewery?.ClassD,
                tel: Convert.ToInt32(brewery?.NumB),
                url: brewery?.ClassE
            );
        }
    }
}
