using Microsoft.EntityFrameworkCore;

namespace offlineMeeting.Models.DBEntity
{
    [PrimaryKey(nameof(EventNumber), nameof(GameNumber), nameof(HandNumber), nameof(HandSubNumber), nameof(UserCd))]
    public class InGameUsersDataEntity
    {
        public int EventNumber { get; set; }
        public int GameNumber { get; set; }
        public int HandNumber { get; set; }
        public int HandSubNumber { get; set; }
        public int UserCd { get; set; }
        public int DirectionCd { get; set; }
        public bool IsParent { get; set; }
        // 0:none 1:ツモ 2:ロン  3:聴牌 4:ノーテン 5:放銃  6:責任払い(ツモ) 7:責任払い(ロン)
        public int ResultCd { get; set; }
        public int Point { get; set; }
        public bool IsLeach {  get; set; }
        public bool IsMyao { get; set; }
        public int GetLeachCount { get; set; }

        public InGameUsersDataEntity(
            int eventNumber,
            int gameNumber,
            int handNumber,
            int handSubNumber,
            int userCd,
            int directionCd,
            bool isParent,
            int resultCd = 0,
            int point = 0,
            bool isLeach = false,
            bool isMyao = false,
            int getLeachCount = 0
        ) {
            EventNumber = eventNumber;
            GameNumber = gameNumber;
            HandNumber = handNumber;
            HandSubNumber = handSubNumber;
            UserCd = userCd;
            DirectionCd = directionCd;
            IsParent = isParent;
            ResultCd = resultCd;
            Point = point;
            IsLeach = isLeach;
            IsMyao = isMyao;
            GetLeachCount = getLeachCount;
        }
    }
}
