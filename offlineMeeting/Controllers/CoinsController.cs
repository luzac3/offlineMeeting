using offlineMeeting.Models;
using offlineMeeting.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using offlineMeeting.Models.Entity.Login;
using offlineMeeting.Models.Process.Login;
using System.Threading.Tasks;

namespace offlineMeeting.Controllers
{
    public class CoinsController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public CoinsController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index([FromQuery] int UserId)
        {
            GenerateQr generateQr = new (userId: UserId);
            LoginViewModel loginViewModel = new ();
            loginViewModel.GenerateQr = generateQr;
            return View(loginViewModel);
        }

        [HttpGet]
        public IActionResult Qr([FromQuery] string uid, string eid)
        {
            GenerateQr generateQr = new (userId: Int32.TryParse(uid, out int parsedUid) ? parsedUid : 0);
            LoginViewModel loginViewModel = new ();
            loginViewModel.GenerateQr = generateQr;
            return View(loginViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> QrRegister([FromQuery] string uid)
        {
            SendRegisterProcess sendRegisterProcess = new (userCd: uid);
            RegisterViewModel registerViewModel = new (await sendRegisterProcess.Send());

            return View(registerViewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}