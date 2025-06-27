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
        public IActionResult ChangePassword(ChangePassword change)
        {
            if (!ModelState.IsValid)
            {
                return View(change);
            }

            // Lấy user đang đăng nhập
            using (var context = new EFCoreDbContext())
            {
                var user = context.UserAccount.FirstOrDefault(u => u.UserName == change.UserName);
                if (user == null)
                {
                    ModelState.AddModelError("", "Không tìm thấy người dùng.");
                    return View(change);
                }

                if (user.Password != change.CurrentPassword)
                {
                    ModelState.AddModelError("", "Mật khẩu hiện tại không đúng.");
                    return View(change);
                }

                // Cập nhật mật khẩu mới
                user.Password = change.NewPassword;
                context.SaveChanges();
            }

            return RedirectToAction("Main", "Home");
        }

    }
}
