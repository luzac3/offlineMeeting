using offlineMeeting.Models.DBProperty;
using offlineMeeting.Models.Entity.Pos;
using offlineMeeting.Models.Process.DBProcess;
using offlineMeeting.Models.Process.Share;

namespace offlineMeeting.Models.Process.Pos
{
    public class SetFoodDataEntityListProcess : ResultsProcess
    {
        private int EventFoodSiteId;
        private int OrderSiteId;
        private string EventId;
        private string UserCd;
        List<FoodDataEntity> FoodDataEntityList;

        public SetFoodDataEntityListProcess(int eventFoodSiteId, int orderSiteId, string eventId, string userCd)
        {
            EventFoodSiteId = eventFoodSiteId;
            OrderSiteId = orderSiteId;
            EventId = eventId;
            UserCd = userCd;
            FoodDataEntityList = new List<FoodDataEntity> { };
        }

        public List<FoodDataEntity> Get()
        {
            List<ResultsEntity> results = base
                .GetFromSiteId(EventFoodSiteId)
                .Where(x=>x.ClassD == EventId)
                .ToList();

            GetFoodDataEntityList(results);

            return FoodDataEntityList;
        }

        public List<FoodDataEntity> Get(List<int> resultId)
        {
            List<ResultsEntity> results = base
                .GetFromSiteId(EventFoodSiteId, resultId)
                .Where(x => x.ClassD == EventId)
                .ToList();

            GetFoodDataEntityList(results);

            return FoodDataEntityList;
        }

        private void GetFoodDataEntityList(List<ResultsEntity> results)
        {
            /*
            List<ResultsEntity> categories = new List<ResultsEntity>();
            if (results.Count > 0)
            {
                if (int.TryParse(results[0].ClassB, out int classBInt))
                {
                    // wikiからの取得になってないので今回はオミット
                    categories = base.GetFromSiteId(classBInt);
                }
            }
            */

            List<int> orderedResultsIdList = base
                .GetFromSiteId(OrderSiteId)
                .Where(x => x.ClassA == EventId)
                .Where(x => x.ClassB == UserCd)
                .Select(x => Convert.ToInt32(x.ClassC ?? "0"))
                .ToList();

            foreach (ResultsEntity result in results)
            {
                string foodName = "";
                if (!string.IsNullOrEmpty(result.ClassA))
                {
                    ResultsEntity? foodEntity = base.GetSingleRecord(result.ClassA);
                    foodName = foodEntity?.Title ?? "";
                }

                string[]? imageSplit = result!.Body?.Split("/");
                string guid = "";
                if (imageSplit != null && imageSplit?.Length > 2)
                {
                    guid = string.IsNullOrEmpty(imageSplit?[2]) ? "" : imageSplit[2];
                }

                GetBinariesProcess getBinariesProcess = new GetBinariesProcess();
                string? image = getBinariesProcess.GetImage(guid);
                FoodDataEntityList
                    .Add(
                        new FoodDataEntity(
                            resultId: result.ResultId,
                            image: image == null ? "" : $"data:image/jpeg;base64,{image}",
                            foodName: foodName,
                            cookingCategory: "",
                            coinNumber: Convert.ToInt32(result.NumG),
                            leftAmount: Convert.ToInt32(result.NumE),
                            ordered: orderedResultsIdList.Contains(Convert.ToInt32(result.ResultId))
                        )
                    );
            }
        }
    }
}
