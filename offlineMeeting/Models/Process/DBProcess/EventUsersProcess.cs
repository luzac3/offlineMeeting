using offlineMeeting.Models.DBProperty;
using offlineMeeting.Models.Share;
using Microsoft.EntityFrameworkCore;

namespace offlineMeeting.Models.Process.DBProcess
{
    public class EventUsersProcess : DBContext
    {
        public DbSet<EventUsersEntity> EventUsers { get; set; }
        public DbSet<UsersEntity> Users { get; set; }

        public EventUsersProcess() { }

        public List<EventUsersEntity> Get()
        {
            return EventUsers.ToList();
        }

        public List<EventUsersEntity> Get(
            int eventNumber
        )
        {
            return EventUsers
                .Where(x=>x.EventNumber == eventNumber)
                .ToList();
        }

        public List<EventUsersEntity> Get(
            int eventNumber,
            List<int> usersCdList
        )
        {
            return EventUsers
                .Where(
                    x => x.EventNumber == eventNumber &&
                    usersCdList.Contains(x.UserCd!)
                )
                .ToList();
        }

        public List<UsersEntity> GetEventUsers(int eventNumber)
        {
            return  EventUsers
                .Where(x=>x.EventNumber == eventNumber)
                .Join(
                    Users,
                    x => x.UserCd,
                    y => y.UserCd,
                    (eu, u) => new UsersEntity(
                        eu.UserCd,
                        u.UserName ?? "",
                        u.UserTitle ?? ""
                    )
                ).ToList();
        }

        public bool Insert(int eventNumber, List<UsersEntity> usersEntityList)
        {
            foreach (UsersEntity usersEntity in usersEntityList)
            {
                EventUsers.Add(
                    new EventUsersEntity(
                        eventNumber: eventNumber,
                        userCd: usersEntity.UserCd
                    )
                );
            };
            SaveChanges();
            return true;
        }

        /*
        public bool Insert(string? tsvUsers)
        {
            List<string> users = tsvUsers?.Split(',').ToList<string>() ??
                new List<string> { };

            return Insert(users);
        }

        public bool Insert(List<string> users)
        {
            List<UsersEntity> usersEntityList = new List<UsersEntity> { };

            var newUserCd = EventUsers.Max(item => item.UserCd) + 1;

            foreach (string newUser in users)
            {
                usersEntityList.Add(
                    new UsersEntity(
                        userCd: newUserCd,
                        userName: newUser
                    )
                );
                newUserCd++;
            }

            try
            {
                if (usersEntityList.Count != 0)
                {
                    EventUsers.AddRange(usersEntityList);
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
        */
    }
}