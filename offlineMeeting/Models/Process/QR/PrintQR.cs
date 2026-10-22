using offlineMeeting.Models.ViewModel;
using PleasanterBridge.src.DataRepository.Entity;
using PleasanterBridge.src.DataRepository.Service;

namespace offlineMeeting.Models.Process.QR
{
    /// <summary>
    /// スタート画面からQR画面を呼び出す前の開始処理。
    /// 今後、ここに開始前の処理を挟み込む予定。
    /// </summary>
    public class PrintQR
    {
        private readonly HttpContext _httpContext;
        private readonly long _checkinSiteId;
        private readonly IPleasanterRepository _repository;

        public PrintQR(HttpContext httpContext, IPleasanterRepository pleasanterRepository, long checkinSiteId)
        {
            _httpContext = httpContext;
            _repository = pleasanterRepository;
            _checkinSiteId = checkinSiteId;
        }

        /// <summary>
        /// 開始前処理を実行する。EventIdをセッションへ保持し、QR画面側で利用できるようにする。
        /// </summary>
        public PrintQRViewModel Execute(long userCd)
        {
            TablesEntity result = _repository.Select(userCd, true).FirstOrDefault() ?? throw new Exception("Result not found");

            PrintQRViewModel viewModel = new(
                result.ClassB ?? "NoName", 
                result.ClassC ?? ""
            );

            return viewModel;
        }
    }
}
