using offlineMeeting.Models.DBProperty;
using offlineMeeting.Models.Entity.Share;
using offlineMeeting.Models.Process.DBProcess;
using System.Text.Json;

namespace offlineMeeting.Models.Process.Share
{
    public class SetAlcoholDataEntityProcess : ResultsProcess
    {
        private int SakeSiteId;
        private int OrderSiteId;
        private string EventId;
        private string UserCd;
        List<AlcoholDataEntity> AlcoholDataEntityList;

        public SetAlcoholDataEntityProcess(int sakeSiteId, int orderSiteId, string eventId, string userCd)
        {
            SakeSiteId = sakeSiteId;
            OrderSiteId = orderSiteId;
            EventId = eventId;
            UserCd = userCd;
            AlcoholDataEntityList = new List<AlcoholDataEntity> { };
        }

        public List<AlcoholDataEntity> Get(System.Text.Json.Nodes.JsonNode? json)
        {
            Dictionary<string, string> choicesTextDic = GetChoicesTextDic(json);

            List<ResultsEntity> results = 
                GetFromSiteId(SakeSiteId)
                .Where(x =>
                    !string.IsNullOrEmpty(x.ClassH) &&
                    JsonSerializer.Deserialize<string[]>(x.ClassH)?.Contains(EventId) == true
                )
                .ToList();

            GetAlcoholDataEntityList(results, choicesTextDic);

            return AlcoholDataEntityList;
        }

        public List<AlcoholDataEntity> Get(System.Text.Json.Nodes.JsonNode? json, List<int> resultId)
        {
            Dictionary<string, string> choicesTextDic = GetChoicesTextDic(json);

            List<ResultsEntity> results = 
                GetFromSiteId(SakeSiteId, resultId)
                .Where(x =>
                    !string.IsNullOrEmpty(x.ClassH) &&
                    JsonSerializer.Deserialize<string[]>(x.ClassH)?.Contains(EventId) == true
                )
                .ToList();

            GetAlcoholDataEntityList(results, choicesTextDic);

            return AlcoholDataEntityList;
        }

        public AlcoholDataEntity Get(System.Text.Json.Nodes.JsonNode? json, int resultId)
        {
            Dictionary<string, string> choicesTextDic = GetChoicesTextDic(json);

            ResultsEntity result = 
                GetSingleRecord(resultId.ToString());

            GetAlcoholDataEntity(result, choicesTextDic);

            return GetAlcoholDataEntity(result, choicesTextDic);
        }

        public List<int> GetHidedIdList()
        {
            return GetFromSiteId(SakeSiteId)
                .Where(x => x.CheckZ ?? false)
                .Select(x => Convert.ToInt32(x.ResultId))
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

        private void GetAlcoholDataEntityList(List<ResultsEntity> results, Dictionary<string, string> choicesTextDic)
        {
            List<int> orderedResultsIdList = 
                GetFromSiteId(OrderSiteId)
                .Where(x => x.ClassA == EventId)
                .Where(x => x.ClassB == UserCd)
                .Select(x => Convert.ToInt32(x.ClassC ?? "0"))
                .ToList();

            foreach (ResultsEntity result in results)
            {
                ResultsEntity? brewery = string.IsNullOrEmpty(result.ClassC) ? null : GetSingleRecord(result.ClassC);
                string? breweryName = brewery?.Title ?? null;
                string? prefectures = string.IsNullOrEmpty(brewery?.ClassB) ? null : GetSingleRecord(brewery.ClassB).ClassA;
                string[]? imageSplit = result!.DescriptionG?.Split("/");
                string guid = "";
                if (imageSplit != null && imageSplit?.Length > 2)
                {
                    guid = string.IsNullOrEmpty(imageSplit?[2]) ? "" : imageSplit[2];
                }

                GetBinariesProcess getBinariesProcess = new GetBinariesProcess();
                string? image = getBinariesProcess.GetImage(guid);
                AlcoholDataEntityList
                    .Add(
                        new AlcoholDataEntity(
                            resultId: result.ResultId,
                            image: image == null ? "" : $"data:image/jpeg;base64,{image}",
                            alcoholName: result.ClassA,
                            abreviation: result.ClassI,
                            brewery: breweryName,
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
                            ordered: orderedResultsIdList.Contains(Convert.ToInt32(result.ResultId)),
                            taseting: result.DescriptionE?.Replace("\n", "<br>")
                        )
                    );
            }
        }

        private AlcoholDataEntity GetAlcoholDataEntity(ResultsEntity result, Dictionary<string, string> choicesTextDic)
        {
            List<int> orderedResultsIdList = 
                GetFromSiteId(OrderSiteId)
                .Where(x => x.ClassA == EventId)
                .Where(x => x.ClassB == UserCd)
                .Select(x => Convert.ToInt32(x.ClassC ?? "0"))
                .ToList();

            ResultsEntity? brewery = string.IsNullOrEmpty(result.ClassC) ? null : GetSingleRecord(result.ClassC);
            string? prefectures = string.IsNullOrEmpty(brewery?.ClassB) ? null : GetSingleRecord(brewery.ClassB).ClassA;
            string[]? imageSplit = result!.DescriptionG?.Split("/");
            string guid = "";
            if (imageSplit != null && imageSplit?.Length > 2)
            {
                guid = string.IsNullOrEmpty(imageSplit?[2]) ? "" : imageSplit[2];
            }

            GetBinariesProcess getBinariesProcess = new GetBinariesProcess();
            string? image = getBinariesProcess.GetImage(guid);

            return new AlcoholDataEntity(
                resultId: result.ResultId,
                image: image == null ? "" : $"data:image/jpeg;base64,{image}",
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
                ordered: orderedResultsIdList.Contains(Convert.ToInt32(result.ResultId)),
                taseting: result.DescriptionE?.Replace("\n", "<br>"),
                rarityReason: result.DescriptionF?.Replace("\n", "<br>"),
                explanation: result.DescriptionC?.Replace("\n", "<br>"),
                mainLabel: brewery?.DescriptionA?.Replace("\n", "<br>"),
                breweryFeatures: brewery?.DescriptionB?.Replace("\n", "<br>"),
                postCode: Convert.ToInt32(brewery?.NumA),
                municipality: brewery?.ClassC,
                address: brewery?.ClassD,
                tel: Convert.ToInt32(brewery?.NumB),
                url: brewery?.ClassE
            );
        }
    }
}
