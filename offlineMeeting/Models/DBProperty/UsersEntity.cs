using Microsoft.EntityFrameworkCore;

namespace offlineMeeting.Models.DBProperty
{
    [PrimaryKey(nameof(UserCd))]
    public class UsersEntity
    {
        public int UserCd { get; set; }
        public string? UserName { get; set; }
        public string? UserTitle { get; set; }
        public UsersEntity() { }
        public UsersEntity(int userCd, string userName)
        {
            UserCd = userCd;
            UserName = userName;
        }
        public UsersEntity(
            int userCd, 
            string userName, 
            string userTitle
        )
        {
            UserCd = userCd;
            UserName = userName;
            UserTitle = userTitle;
        }
    }
}
