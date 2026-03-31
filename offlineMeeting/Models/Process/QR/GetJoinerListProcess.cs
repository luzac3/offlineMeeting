using offlineMeeting.Models.Entity.Share;
using offlineMeeting.Models.Share;
using PleasanterBridge.src.DataRepository.Entity;
using PleasanterBridge.src.DataRepository.Service;

namespace offlineMeeting.Models.Process.QR
{
    public class GetJoinerListProcess
    {
        private readonly IPleasanterRepository _repository;
        private readonly long _eventId;

        public GetJoinerListProcess(HttpContext httpContext, IPleasanterRepository pleasanterRepository)
        {
            ManageEventId manageEventId = new ManageEventId(httpContext);
            _eventId = manageEventId.GetEventId();
            _repository = pleasanterRepository;
        }

        public List<JoinerEntity> Get(long siteId)
        {
            // usecdのリストを取得
            List<long> userCdList = _repository
                .Select(siteId, true)
                .Where(x => x.ClassA == _eventId.ToString())
                .Select(x => long.Parse(x.ClassB!))
                .ToList();

            List<TablesEntity> tablesEntities = _repository.Select(userCdList, true);

            return tablesEntities.Select(t => new JoinerEntity(
                eventId: t.ClassD,
                userCd: t.ReferenceId.ToString(),
                userName: t.ClassA ?? "",
                sex: t.ClassB ?? "",
                contact: t.ClassC ?? "",
                handoverNotes: t.DescriptionA?[0],
                coins: Convert.ToInt32(t.NumA ?? 0),
                exp: Convert.ToInt32(t.NumB ?? 0),
                expNext: Convert.ToInt32(t.NumD ?? 0),
                level: Convert.ToInt32(t.NumC ?? 1),
                isAdmin: t.CheckB ?? false
            )).ToList();
        }
    }
}