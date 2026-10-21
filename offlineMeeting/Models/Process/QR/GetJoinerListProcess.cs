using offlineMeeting.Models.Entity.QR;
using offlineMeeting.Models.Share;
using PleasanterBridge.src.DataRepository.Entity;
using PleasanterBridge.src.DataRepository.Service;

namespace offlineMeeting.Models.Process.QR
{
    public class GetJoinerListProcess
    {
        private readonly IPleasanterRepository _repository;
        private readonly long _eventId;

        public GetJoinerListProcess(HttpContext httpContext, IPleasanterRepository pleasanterRepository, long eventId = 0)
        {
            if (eventId != 0)
            {
                _eventId = eventId;
            }
            else
            {
                ManageEventId manageEventId = new ManageEventId(httpContext);
                _eventId = manageEventId.GetEventId();
            }
            _repository = pleasanterRepository;
        }

        public List<CheckinJoinerEntity> Get(long siteId)
        {
            // EventIdを条件にサイト内レコードを検索
            var where = new Dictionary<string, object>
            {
                { "ClassA", new string[] { _eventId.ToString() } },
                { "Status", 200 }
            };

            // チェックインレコードを取得（ReferenceId=更新用Id, ClassB=UserCd, ClassC=Url）
            List<TablesEntity> checkinRecords = _repository.Select(siteId, where, true);

            // 該当EventIdの参加者がいない場合は空リストを返す
            if (checkinRecords.Count == 0)
            {
                return new List<CheckinJoinerEntity>();
            }

            List<long> userCdList = checkinRecords
                .Select(x => long.Parse(x.ClassB!))
                .ToList();

            // ユーザマスタから名前を取得
            Dictionary<string, string> userNameMap = _repository
                .Select(userCdList, true)
                .ToDictionary(
                    u => u.ReferenceId.ToString(),
                    u => u.ClassA ?? ""
                );

            var joinerList = checkinRecords.Select(c => new CheckinJoinerEntity(
                id: c.ReferenceId,
                userCd: c.ClassB ?? "",
                name: userNameMap.GetValueOrDefault(c.ClassB ?? "", ""),
                url: c.ClassC ?? "",
                checkedIn: c.CheckB ?? false
            )).ToList();

            // VOICEVOX で読み上げ（補助機能のため失敗しても本処理に影響させない）
            // メッセージ内容は仮置き。後で name 等を渡す想定。
            VoiceVoxProcess voiceVoxProcess = new();
            _ = voiceVoxProcess.Execute("こんにちはなのだ。　名前を選んでほしいのだ");

            return joinerList;
        }
    }
}
