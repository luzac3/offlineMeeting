namespace offlineMeeting.Models.Entity.GameData
{
    public class EndHandRegisterPostEntity
    {
        public int EventNumber { get; set; }
        public int HoraKind { get; set; }
        public int Parent { get; set; }
        // [toUser, fromUser, point1]
        public List<Dictionary<string, int>>? RonPointDicList { get; set; }
        // [toUser, point1, point2]
        public Dictionary<string, int>? PickPointDic { get; set; }
        // [paoToUser, paoFromUser, paoPoint]
        public List<Dictionary<string, int>>? PaoPointDicList { get; set; }
        public List<int>? TenpaiUsersList { get; set; }
        public List<int>? LeachUsersList { get; set; }
        public List<int>? MyaoUsersList { get; set; }
        public Dictionary<int, string>? VideoIdDic { get; set; }
        public Dictionary<int, string>? VideoNameDic { get; set; }
        public Dictionary<int, bool>? ValidVideoDic { get; set; }

        public EndHandRegisterPostEntity() { }
    }
}