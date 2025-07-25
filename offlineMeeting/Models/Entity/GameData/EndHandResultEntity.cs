namespace offlineMeeting.Models.Entity.GameData
{
    public class EndHandResultEntity
    {
        public int UserCd { get; set; }
        public int Direction { get; set; }
        public int NowHandNumber { get; set; }
        public int NowHandSubNumber { get; set; }
        public int NextHandNumber { get; set; }
        public int NextHandSubNumber { get; set; }
        public int ResultCd { get; set; }
        public int MovingPoint { get; set; }
        public int Point { get; set; }
        public int NowLeachCount { get; set; }
        public int NextLeachCount { get; set; }
        public bool IsLeach {  get; set; }
        public bool IsMyao { get; set; }
        public string UserName { get; set; }
        public string UserTitle { get; set; }
        public int? VideoNumber { get; set; }


        public EndHandResultEntity(
            int userCd,
            int direction,
            int nowHandNumber,
            int nowHandSubNumber,
            int nextHandNumber,
            int nextHandSubNumber,
            int resultCd,
            int movingPoint,
            int point,
            int nowLeachCount,
            int nextLeachCount,
            bool isLeach,
            bool isMyao,
            string userName,
            string userTitle,
            int? videoNumber
        ) {
            UserCd = userCd;
            Direction = direction;
            NowHandNumber = nowHandNumber;
            NowHandSubNumber = nowHandSubNumber;
            NextHandNumber = nextHandNumber;
            NextHandSubNumber = nextHandSubNumber;
            ResultCd = resultCd;
            MovingPoint = movingPoint;
            Point = point;
            NowLeachCount = nowLeachCount;
            NextLeachCount = nextLeachCount;
            IsLeach = isLeach;
            IsMyao = isMyao;
            UserName = userName;
            UserTitle = userTitle;
            VideoNumber = videoNumber;
        }
    }
}
