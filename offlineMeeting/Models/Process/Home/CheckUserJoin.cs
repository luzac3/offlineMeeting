using PleasanterBridge.src.DataRepository.Entity;
using PleasanterBridge.src.DataRepository.Service;

namespace offlineMeeting.Models.Process.Home
{
    public class CheckUserJoin
    {
        private readonly IPleasanterRepository _pleasanterRepository;
        private long UserCd;
        public CheckUserJoin(IPleasanterRepository pleasanterRepository, long userCd) {
            _pleasanterRepository = pleasanterRepository;
            UserCd = userCd;
        }

        public bool Check()
        {
            TablesEntity? tablesEntity = _pleasanterRepository.Select(UserCd, true).FirstOrDefault();
            return tablesEntity != null && tablesEntity.CheckA == true;
        }
    }
}
