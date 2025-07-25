using offlineMeeting.Models;
using offlineMeeting.Models.Entity.EventList;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using offlineMeeting.Models.Process.EventList;
using offlineMeeting.Models.ViewModel;
using offlineMeeting.Models.Process.DBProcess;

namespace offlineMeeting.Controllers
{
    public class EventListController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private EventListViewModel EventListViewModel;

        public EventListController(ILogger<HomeController> logger)
        {
            _logger = logger;
            EventListViewModel = new EventListViewModel();
        }

        public IActionResult Index()
        {
            // DBからEventListを取得
            EventDataProcess eventDataProcess = new EventDataProcess();

            // UserListを取得
            UsersProcess userProcess = new UsersProcess();

            // List化してViewModelにデータを流し込み
            EventListViewModel.setEventDataEntity(eventDataProcess.Get());
            EventListViewModel.setUsersEntityList(userProcess.Get());
            return View(EventListViewModel);
        }

        [HttpPost]
        public string RegisterEvent([FromBody] RegisterEventPostEntity registerEventPostEntity)
        {
            string returnStatus = "1";
            var registerEventProcess = new RegisterEventProcess();
            try { 
                registerEventProcess.RegisterEvent(registerEventPostEntity);
            }
            catch(Exception e)
            {
                returnStatus = e.ToString();
            }
            return returnStatus;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
