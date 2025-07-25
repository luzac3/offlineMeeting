namespace offlineMeeting.Models.Entity.GameData
{
    public class ShowPointResultEntity
    {
        public int HandNumber { get; set; }
        public int HandSubNumber { get; set; }
        public int UserCd { get; set; }
        public int Direction { get; set; }
        public int Point { get; set; }
        public int NowLeachCount { get; set; }
        // fixme いずれ実装の可能性あり
        //public bool IsLeach {  get; set; }
        //public bool IsMyao { get; set; }
        public string UserName { get; set; }
        public string UserTitle { get; set; }
        public int? VideoNumber { get; set; }


        public ShowPointResultEntity(
            int handNumber,
            int handSubNumber,
            int userCd,
            int direction,
            int point,
            int nowLeachCount,
            //bool isLeach,
            //bool isMyao,
            string userName,
            string userTitle,
            int? videoNumber
        ) {
            HandNumber = handNumber;
            HandSubNumber = handSubNumber;
            UserCd = userCd;
            Direction = direction;
            Point = point;
            NowLeachCount = nowLeachCount;
            //IsLeach = isLeach;
            //IsMyao = isMyao;
            UserName = userName;
            UserTitle = userTitle;
            VideoNumber = videoNumber;
        }
    }
}
