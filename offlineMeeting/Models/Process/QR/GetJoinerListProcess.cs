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
            // EventIdを条件にサイト内レコードを検索
            var where = new Dictionary<string, object>
            {
                { "ClassA", new string[] { _eventId.ToString() } },
                { "Status", 200 }
            };

            // usecdのリストを取得
            List<long> userCdList = _repository
                .Select(siteId, where, true)
                .Select(x => long.Parse(x.ClassB!))
                .ToList();

            // 該当EventIdの参加者がいない場合は空リストを返す
            if (userCdList.Count == 0)
            {
                return new List<JoinerEntity>();
            }

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