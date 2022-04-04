using M133_M151_Jubilaeum_der_Musikkapelle_Seetalhorn.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace M133_M151_Jubilaeum_der_Musikkapelle_Seetalhorn.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}