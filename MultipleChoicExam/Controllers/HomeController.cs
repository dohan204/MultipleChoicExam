using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MultipleChoicExam.Models;

namespace MultipleChoicExam.Controllers
{
    public class HomeController : Controller
    {
        private readonly EFCoreDbContext _dbContext;

        public HomeController(EFCoreDbContext dbContext)
        {
            _dbContext = dbContext;
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
              var userExits = _dbContext.UserAccount.FirstOrDefault(u => u.UserName == user.UserName && u.Password == user.Password);
                if (userExits != null) 
                {
                    return RedirectToAction("Main", "Home");
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

                var user = _dbContext.UserAccount.FirstOrDefault(u => u.UserName == change.UserName);
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
                _dbContext.SaveChanges();

            return RedirectToAction("Main", "Home");
        }
        [HttpGet]
        public IActionResult ManageUser()
        {
            var user = new newUser
            {
                User = new UserAccount(),
                userList = _dbContext.UserAccount.ToList()
            };
            return View(user);
          
        }
        [HttpGet]
        public IActionResult AddUser()
        {
            return View(new UserAccount());
        }

        [HttpPost]
        public IActionResult AddUser(UserAccount model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var add = new UserAccount()
            {
                UserName = model.UserName,
                Password = model.Password,
                RoleId = model.RoleId,
                Birthday = model.Birthday,
                PhoneNumber = model.PhoneNumber,
                FullName = model.FullName,
                Email = model.Email,
                Address = model.Address,
            };
            _dbContext.UserAccount.Add(add);
            _dbContext.SaveChanges();

            return RedirectToAction("ManageUser");
        }

        [HttpGet]
        public IActionResult EditUser(int id)
        {
            var editUser = _dbContext.UserAccount.FirstOrDefault(u => u.UserId == id);
            if(editUser == null)
            {
                return View(new UserAccount());
            }
            return View(editUser);
        }

        [HttpPost]
        public IActionResult EditUser(UserAccount model)
        {
            var user = _dbContext.UserAccount.FirstOrDefault(u => u.UserId == model.UserId);
            if(user != null)
            {
                user.UserName = model.UserName;
                user.Password = model.Password;
                user.RoleId = model.RoleId;
                user.Birthday = model.Birthday;
                user.PhoneNumber = model.PhoneNumber;
                user.FullName = model.FullName;
                user.Email = model.Email;
                user.Address = model.Address;
            }
            _dbContext.SaveChanges();
            return RedirectToAction("ManageUser");
        }

        [HttpGet]

        public IActionResult DeleteUser(int id) 
        {
            var dele = _dbContext.UserAccount.FirstOrDefault( u => u.UserId == id);
            if (dele == null) 
            {
                return NotFound(); // nếu bằng null thì hiển thị tb không tìm thấy
            }
            return View(dele); // trả về user
        }

        [HttpPost, ActionName("DeleteUser")] // tên post thì vẫn là DeleteUser, nma method thì là cái kia để cho dễ phân biết
        public IActionResult ComfinDelete(UserAccount abc)
        {
            var deleUser = _dbContext.UserAccount.FirstOrDefault( u=> u.UserId == abc.UserId);
            if (deleUser != null) // nếu khác null là đã lấy đc user
            {
                // thực hiện xóa user
                _dbContext.UserAccount.Remove(deleUser); // truy cập vào table useraccount

                // lưu lại
                _dbContext.SaveChanges();
            }
            // xóa xong thì trả về view đang hiện thị danh sách
            return RedirectToAction("ManageUser");
        }
        [HttpPost]
        public IActionResult SearchInforResult(newUser model)
        {
            Console.WriteLine(">>> UserName nhận được: " + model.User.UserName);

            if (string.IsNullOrEmpty(model.User.UserName))
            {
                return BadRequest("Thiếu UserName");
            }
            var foundUser = _dbContext.UserAccount
                .FirstOrDefault(u => u.UserName == model.User.UserName);
            if(foundUser == null)
            {
                return NotFound();
            }
            return View(foundUser);
        }



    }
}
