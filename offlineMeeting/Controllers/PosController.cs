using Microsoft.AspNetCore.Mvc;
using offlineMeeting.Models;
using offlineMeeting.Models.Entity.Pos;
using offlineMeeting.Models.JsonDataProperty;
using offlineMeeting.Models.Process.Pos;
using offlineMeeting.Models.Process.Share;
using offlineMeeting.Models.ViewModel;
using System.Diagnostics;

namespace offlineMeeting.Controllers
{
    public class PosController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private PosViewModel PosViewModel;
        private int SakeSiteId;
        private string hyakusyuEventId = "1";

        public PosController(ILogger<HomeController> logger)
        {
            _logger = logger;
            PosViewModel = new PosViewModel();
            TableIdProperty tableIdProperty = new TableIdProperty();
            SakeSiteId = tableIdProperty.SakeSiteId;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Appetizers()
        {
            return View();
        }

        public IActionResult Alcohol()
        {
            AlcoholDataProcess alcoholDataProcess = new AlcoholDataProcess(sakeSiteId: SakeSiteId);
            SiteSettingsProcess siteSettingsProcess = new SiteSettingsProcess(sakeSiteId: SakeSiteId);

            PosViewModel.setAlcoholDataList(alcoholDataProcess.Get(siteSettingsProcess.Get(), hyakusyuEventId));
            return View(PosViewModel);
        }

        public IActionResult OrderList()
        {
            return View();
        }

        [HttpPost]
        public IActionResult OrderListContents([FromBody] List<OrderPostEntity> orderPostEntityList)
        {
            OrderListProcess orderListProcess = new OrderListProcess(orderPostEntityList: orderPostEntityList, sakeSiteId: SakeSiteId);
            SiteSettingsProcess siteSettingsProcess = new SiteSettingsProcess(sakeSiteId: SakeSiteId);

            PosViewModel.setOrderEntityList(orderListProcess.Get(siteSettingsProcess.Get(), hyakusyuEventId));
            return PartialView("_OrderListContents", PosViewModel);
        }

        [HttpPost]
        public string SendOrder([FromBody] OrderPostEntity orderPostEntityList) // SendOrderPostEntityに修正予定
        {
            // orderを作成してデータを投げる
            // pleasanterの仕様上、更新は1行ずつしかできない・・・BulkUpdate利かないんだよな
            string returnStatus = "1";
            return returnStatus;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
