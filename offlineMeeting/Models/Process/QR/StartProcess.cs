using offlineMeeting.Models.Share;
using PleasanterBridge.src.DataRepository.Service;

namespace offlineMeeting.Models.Process.QR
{
    /// <summary>
    /// スタート画面からQR画面を呼び出す前の開始処理。
    /// 今後、ここに開始前の処理を挟み込む予定。
    /// </summary>
    public class StartProcess
    {
        private readonly HttpContext _httpContext;
        private readonly IPleasanterRepository _repository;
        private readonly long _checkinSiteId;

        public StartProcess(HttpContext httpContext, IPleasanterRepository pleasanterRepository, long checkinSiteId)
        {
            _httpContext = httpContext;
            _repository = pleasanterRepository;
            _checkinSiteId = checkinSiteId;
        }

        /// <summary>
        /// 開始前処理を実行する。EventIdをセッションへ保持し、QR画面側で利用できるようにする。
        /// </summary>
        public void Execute(long eid)
        {
            // VOICEVOX で読み上げ（補助機能のため失敗しても本処理に影響させない）
            VoiceVoxProcess voiceVoxProcess = new();
            _ = voiceVoxProcess.Execute("いらっしゃいませなのだ");

            ManageEventId manageEventId = new(_httpContext);

            if (eid != 0)
            {
                manageEventId.SetEventId(eid);
            }
        }
    }
}
