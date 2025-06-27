using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MultipleChoicExam.Models;

namespace MultipleChoicExam.Controllers
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
        public IActionResult Main()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(UserAccount user)
        {
            using (var users = new EFCoreDbContext()) 
            {
                var userExits = users.UserAccount.FirstOrDefault(u => u.UserName == user.UserName && u.Password == user.Password);
                if (userExits != null) 
                {
                    return RedirectToAction("Main", "Home");
                }
            }
            return View(user);
        }
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ChangePassoword(UserAccount user) 
        {
            return View(user);
        }
    }
}
