using offlineMeeting.Models.Entity.Pos;

namespace offlineMeeting.Models.Process.Pos
{
    public class AlcoholDataProcess
    {
        private int SakeSiteId;

        public AlcoholDataProcess(int sakeSiteId) {
            SakeSiteId = sakeSiteId;
        }

        public List<AlcoholDataEntity> Get(System.Text.Json.Nodes.JsonNode? json, string eventId)
        {
            SetAlcoholDataEntityListProcess setAlcoholDataEntityListProcess = new SetAlcoholDataEntityListProcess(SakeSiteId);
            return setAlcoholDataEntityListProcess.Get(json, eventId);
        }
    }
}