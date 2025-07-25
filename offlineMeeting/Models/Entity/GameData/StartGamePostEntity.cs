namespace offlineMeeting.Models.Entity.GameData
{
    public class StartGamePostEntity
    {
        public int EventNumber { get; set; }
        public int? HandNumber { get; set; }
        public int? HandSubNumber { get; set; }
        public List<int>? UsersList { get; set; }
        public Dictionary<int, int>? DirectionDictionary { get; set; }
        public Dictionary<int, string>? VideoDictionary { get; set; }
        public Dictionary<int, int>? VideoNumberDictionary { get; set; }

        public StartGamePostEntity() { }
    }
}
