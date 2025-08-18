using offlineMeeting.Models.DBProperty;
using offlineMeeting.Models.Entity.Pos;
using offlineMeeting.Models.Process.DBProcess;
using offlineMeeting.Models.Process.Share;
using offlineMeeting.Models.Share;

namespace offlineMeeting.Models.Process.Pos
{
    public class SetAlcoholDataEntityListProcess : ResultsProcess
    {
        private int SakeSiteId;
        private string EventId;
        List<AlcoholDataEntity> AlcoholDataEntityList;

        public SetAlcoholDataEntityListProcess(int sakeSiteId, string eventId) {
            EventId = eventId;
            SakeSiteId = sakeSiteId;
            AlcoholDataEntityList = new List<AlcoholDataEntity> { };
        }

        public List<AlcoholDataEntity> Get(System.Text.Json.Nodes.JsonNode? json)
        {
            Dictionary<string, string> choicesTextDic = GetChoicesTextDic(json);

            List<ResultsEntity> results = base
                .GetFromSiteId(SakeSiteId)
                .Where(x=>x.ClassH == EventId)
                .ToList();

            GetAlcoholDataEntityList(results, choicesTextDic);

            return AlcoholDataEntityList;
        }

        public List<AlcoholDataEntity> Get(System.Text.Json.Nodes.JsonNode? json, List<int> resultId)
        {
            Dictionary<string, string> choicesTextDic = GetChoicesTextDic(json);

            List<ResultsEntity> results = base
                .GetFromSiteId(SakeSiteId, resultId)
                .Where(x => x.ClassH == EventId)
                .ToList();

            GetAlcoholDataEntityList(results, choicesTextDic);

            return AlcoholDataEntityList;
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
            foreach (ResultsEntity result in results)
            {
                ResultsEntity? brewery = int.TryParse(result.ClassC, out int breweryId) ? base.GetSingleRecord(breweryId) : null;
                string? breweryName = brewery?.Title;
                string? prefectures = int.TryParse(brewery?.ClassB, out int prefectureId) ? base.GetSingleRecord(prefectureId).ClassA : null;
                string[]? imageSplit = result.DescriptionG?.Split("/");
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
                            leftAmount: Convert.ToInt32(result.NumH)
                        )
                    );
            }
        }
    }
}
