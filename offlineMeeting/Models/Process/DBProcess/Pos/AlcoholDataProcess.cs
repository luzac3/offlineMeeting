using offlineMeeting.Models.DBProperty;
using offlineMeeting.Models.Entity.Pos;
using offlineMeeting.Models.Share;

namespace offlineMeeting.Models.Process.DBProcess.Pos
{
    public class AlcoholDataProcess :ResultsProcess
    {
        private int SiteId = 123;

        public AlcoholDataProcess() {}

        public List<AlcoholDataEntity> Get(System.Text.Json.Nodes.JsonNode? json)
        {
            var columns = json?["Columns"];

            // 特定名称選択肢
            var choicesTexts = columns?.AsArray()
                .Where(x => x?["ColumnName"]?.GetValue<string>() == "ClassE")
                .Select(s => s?["ChoicesText"]?.GetValue<string>())
                .FirstOrDefault()?
                .Split(",");

            Dictionary<int, string> choicesTextDix = new Dictionary<int, string>();
            if(choicesTexts != null)
            {
                int choicesTextsLength = choicesTexts.Length;
                for (int i = 0; i < choicesTextsLength; i += 2)
                {
                    
                    if(!Int32.TryParse(choicesTexts[i], out int choiceId))
                    {
                        break;
                    }
                    choicesTextDix.Add(choiceId, choicesTexts[i + 1]);
                }
            }

            List<AlcoholDataEntity> alcoholDataEntityList = new List<AlcoholDataEntity> { };
            List<ResultsEntity>  results = base.GetFromSiteId(SiteId);
            foreach (ResultsEntity result in results)
            {
                ResultsEntity? brewery = Int32.TryParse(result.ClassC, out int breweryId) ? base.Get(breweryId) : null;
                string? breweryName = brewery?.Title;
                string? prefectures = Int32.TryParse(brewery?.ClassB, out int prefectureId) ? base.Get(prefectureId).ClassA : null;
                alcoholDataEntityList
                    .Add(
                        new AlcoholDataEntity(
                            alcoholNumber: result.ResultId,
                            alcoholName: result.ClassA,
                            amount: Convert.ToInt32(result.NumA),
                            brewery: breweryName,
                            prefectures: prefectures,
                            genryoMai: result.ClassD,
                            speciallyDesignated: result.ClassE, // 特定名称
                            ricePolishingRatio: Convert.ToInt32(result.NumB),
                            sakeMeterValue: result.NumC,
                            acidRate: result.NumG,
                            abv: result.NumD,
                            rarity: Convert.ToInt32(result.NumF)
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
                alcoholName: result.ClassA,
                amount: Convert.ToInt32(result.NumA),
                brewery: result.ClassC,
                prefectures: string.Empty,
                genryoMai: result.ClassD,
                speciallyDesignated: result.ClassE,
                ricePolishingRatio: Convert.ToInt32(result.NumB),
                sakeMeterValue: result.NumC,
                acidRate: result.NumA,
                abv: result.NumD,
                rarity: Convert.ToInt32(result.NumF)
            );
        }
    }
}
