using offlineMeeting.Models.DBProperty;
using offlineMeeting.Models.Entity.Pos;
using offlineMeeting.Models.Share;

namespace offlineMeeting.Models.Process.DBProcess.Pos
{
    public class AlcoholDataProcess :ResultsProcess
    {
        private int SakeSiteId;

        public AlcoholDataProcess(int sakeSiteId) {
            SakeSiteId = sakeSiteId;
        }

        public List<AlcoholDataEntity> Get(System.Text.Json.Nodes.JsonNode? json, string eventId)
        {
            var columns = json?["Columns"];

            // 特定名称選択肢
            var choicesTexts = columns?.AsArray()
                .Where(x => x?["ColumnName"]?.GetValue<string>() == "ClassE")
                .Select(s => s?["ChoicesText"]?.GetValue<string>())
                .FirstOrDefault() ?
                .Split(new string[] { "\n" }, StringSplitOptions.None);

            Dictionary<string, string> choicesTextDic = new Dictionary<string, string>();

            if(choicesTexts != null)
            {
                foreach (string choicesText in choicesTexts)
                {
                    string[] raw = choicesText.Split(',');
                    choicesTextDic.Add(raw[0], raw.Length < 2 ? "" : raw[1]);
                }
            }

            List<AlcoholDataEntity> alcoholDataEntityList = new List<AlcoholDataEntity> { };
            List<ResultsEntity>  results = base
                .GetFromSiteId(SakeSiteId)
                .Where(x=>x.ClassH == eventId)
                .ToList();
            foreach (ResultsEntity result in results)
            {
                ResultsEntity? brewery = Int32.TryParse(result.ClassC, out int breweryId) ? base.Get(breweryId) : null;
                string? breweryName = brewery?.Title;
                string? prefectures = Int32.TryParse(brewery?.ClassB, out int prefectureId) ? base.Get(prefectureId).ClassA : null;
                string[]? imageSplit = result.DescriptionG?.Split("/");
                string guid = "";
                if (imageSplit != null && imageSplit?.Length > 2)
                {
                    guid = String.IsNullOrEmpty(imageSplit?[2]) ? "" : imageSplit[2];
                }

                GetBinariesProcess getBinariesProcess = new GetBinariesProcess();
                string? image = getBinariesProcess.GetImage(guid);
                alcoholDataEntityList
                    .Add(
                        new AlcoholDataEntity(
                            alcoholNumber: result.ResultId,
                            image: image == null ? "" : $"data:image/jpeg;base64,{image}",
                            alcoholName: result.ClassA,
                            brewery: breweryName,
                            prefectures: prefectures,
                            genryoMai: result.ClassD,
                            speciallyDesignated: String.IsNullOrEmpty(result.ClassE) ? "" : choicesTextDic[result.ClassE], // 特定名称
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
            return alcoholDataEntityList;
        }
        
        public new AlcoholDataEntity Get(int alcoholNumber)
        {
            ResultsEntity result = base.Get(alcoholNumber);
            return new AlcoholDataEntity(
                alcoholNumber: result.ResultId,
                image: result.DescriptionA,
                alcoholName: result.ClassA,
                brewery: result.ClassC,
                prefectures: string.Empty,
                genryoMai: result.ClassD,
                speciallyDesignated: result.ClassE,
                ricePolishingRatio: Convert.ToInt32(result.NumB),
                sakeMeterValue: result.NumC,
                acidRate: result.NumA,
                abv: result.NumD,
                rarity: Convert.ToInt32(result.NumF),
                coinNumber: Convert.ToInt32(result.NumI),
                leftAmount: Convert.ToInt32(result.NumH)
            );
        }
    }
}
