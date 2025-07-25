using Microsoft.EntityFrameworkCore;

namespace offlineMeeting.Models.DBEntity
{
    [PrimaryKey(nameof(EventNumber), nameof(GameNumber), nameof(UserCd))]
    public class GameUsersDataEntity
    {
        public int EventNumber { get; set; }
        public int GameNumber { get; set; }
        public int UserCd { get; set; }
        public int Direction { get; set; }
        public int Point { get; set; }
        public string? VideoId { get; set; }
        public int? VideoNumber { get; set; }
        public string? VideoName { get; set; }
        public bool ValidVideo { get; set; }
        public GameUsersDataEntity() { }
        public GameUsersDataEntity(
            int eventNumber,
            int gameNumber,
            int userCd,
            int direction,
            int point,
            string? videoId = null,
            int? videoNumber = null,
            string? videoName = null,
            bool validVideo = false
        )
        {
            EventNumber = eventNumber;
            GameNumber = gameNumber;
            UserCd = userCd;
            Direction = direction;
            Point = point;
            VideoId = videoId;
            VideoNumber = videoNumber;
            VideoName = videoName;
            ValidVideo = validVideo;
        }
    }
}