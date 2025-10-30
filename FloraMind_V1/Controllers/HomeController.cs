using System.Diagnostics;
using FloraMind_V1.Models;
using Microsoft.AspNetCore.Mvc;
using FloraMind_V1.Data;
namespace FloraMind_V1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly FloraMindDbContext _context;

        public HomeController(ILogger<HomeController> logger , FloraMindDbContext context)
        {
            _logger = logger;
            _context = context;
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
