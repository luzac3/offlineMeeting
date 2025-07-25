using offlineMeeting.Models.DBProperty;
using offlineMeeting.Models.Process.Share;
using offlineMeeting.Models.Share;
using Microsoft.EntityFrameworkCore;

namespace offlineMeeting.Models.Process.DBProcess
{
    public class UsersProcess: DBContext
    {
        public DbSet<UsersEntity> Users { get; set; }

        public UsersProcess() { }

        public List<UsersEntity> Get()
        {
            return Users.ToList();
        }

        public UsersEntity Get(int userCd)
        {
            return Users
                .Where(x => x.UserCd == userCd)
                .FirstOrDefault() ?? throw new Exception("invalid userCd");
        }

        public List<UsersEntity> Get(List<string> usersNameList)
        {
            return GetUsersByName(usersNameList);
        }

        public List<UsersEntity> Get(string? tsvUserNames)
        {
            List<string> usersNameList = tsvUserNames?.Split(',').ToList() ??
                new List<string> { };
            List<int> userCdList;
            try
            {
                userCdList = ChangeStringListToInt.Convert(usersNameList);
                return GetUsersByCd(userCdList);
            }
            catch
            {
                return GetUsersByName(usersNameList);
            }
        }

        public List<UsersEntity> Get(List<int> usersCdList)
        {
            return GetUsersByCd(usersCdList);
        }

        public bool Insert(string? tsvUsers, string? tsvUsersTitle = "")
        {
            List<string> users = tsvUsers?.Split(',').ToList<string>() ??
                new List<string> { };

            List<string> usersTitle = tsvUsersTitle?.Split(',').ToList<string>() ??
                new List<string> { };

            return Insert(users);
        }

        public bool Insert(List<string> users)
        {
            List<UsersEntity> usersEntityList = new List<UsersEntity> { };

            var newUserCd = Users.Max(item => item.UserCd) + 1;

            foreach (string newUser in users)
            {
                usersEntityList.Add(
                    new UsersEntity(
                        userCd: newUserCd,
                        userName: newUser,
                        userTitle: ""
                    )
                );
                newUserCd++;
            }

            try
            {
                if (usersEntityList.Count != 0)
                {
                    Users.AddRange(usersEntityList);
                    SaveChanges();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                throw;
            }
            return true;

        }

        private List<UsersEntity> GetUsersByName(List<string> usersNameList)
        {
            return usersNameList.Count == 0 ?
                new List<UsersEntity>() :
                Users
                .Where((usersEntity) =>
                    usersNameList.Contains(usersEntity.UserName!)
                )
                .ToList();
        }

        private List<UsersEntity> GetUsersByCd(List<int> usersCdList)
        {
            return usersCdList.Count == 0 ?
                new List<UsersEntity>() :
                Users
                .Where((usersEntity) =>
                    usersCdList.Contains(usersEntity.UserCd)
                )
                .ToList();
        }
    }
}