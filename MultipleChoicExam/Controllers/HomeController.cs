using System.Diagnostics;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Identity.Client;
using MultipleChoicExam.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace MultipleChoicExam.Controllers
{
    public class HomeController : Controller
    {
        private readonly EFCoreDbContext _dbContext;
        private int _selectedIndex = 0;
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
        public async Task<IActionResult> Login(UserAccount user)
        {
            var userExits = _dbContext.UserAccount
                .FirstOrDefault(u => u.UserName == user.UserName && u.Password == user.Password);

            if (userExits != null)
            {
                // Tạo claims
                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, userExits.UserName)
        };

                var identity = new ClaimsIdentity(claims, "MyCookieAuth");
                var principal = new ClaimsPrincipal(identity);

                // Đăng nhập
                await HttpContext.SignInAsync("MyCookieAuth", principal);

                return RedirectToAction("Main", "Home");
            }

            ModelState.AddModelError("", "Sai tài khoản hoặc mật khẩu");
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

        //private List<Question> _questions;

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
        public void LoadSubject()
        {
            var subject = _dbContext.Subject01.Select(sub => new SelectListItem {
                Value = sub.SubjectId,
                Text = sub.SubjectName,
            });


            List<SelectListItem> selectListItems = new List<SelectListItem>() { 
                new SelectListItem {Value="20", Text = "20"},
                new SelectListItem {Value="25", Text = "25"},
                new SelectListItem {Value="30", Text = "30"},
                new SelectListItem {Value="40", Text = "40"},
                new SelectListItem {Value="50", Text = "50"},
            };
            ViewBag.Subject = subject;
            ViewBag.Total = selectListItems;

        }
        [HttpGet("Home/Test")]
        public ViewResult Test()
        {
            LoadSubject();
            

            return View();
        }
        [HttpPost("Home/Test")]
        public IActionResult Test(HomeTest test)
        {

            return RedirectToAction("TestStart", new
            {
                subjectid = test.Subject01.SubjectId,
                total = test.TotalQuestion
            });
        }
        //public IActionResult loadQuestion(string subjectid,  int totalQuestion, int selectedIndex)
        //{
        //    var model = TestStart(subjectid, totalQuestion, selectedIndex);
        //    return View(model);
        //}
        [HttpGet]
        public IActionResult TestStart(string subjectid, int total, int? selectedIndex)
        {
            // --- 1. Chuẩn bị danh sách các câu hỏi cho ListBox (QuestionOptions) ---
            List<Question> randomQues;
            var questionIdsFromTempData = TempData.Peek("QuestionIds") as string;

            if (!string.IsNullOrEmpty(questionIdsFromTempData))
            {
                // Nếu đã có danh sách ID câu hỏi từ lần trước, lấy lại các câu hỏi đó
                var ids = questionIdsFromTempData.Split(',').Select(int.Parse).ToList();
                randomQues = _dbContext.Question.Where(q => ids.Contains(q.QuestionId)).ToList();
                // Đảm bảo thứ tự vẫn giống như lúc tạo ra ban đầu
                randomQues = ids.Join(randomQues, id => id, q => q.QuestionId, (id, q) => q).ToList();
            }
            else
            {
                // Lần đầu tải trang hoặc không có ID lưu trữ, tạo danh sách ngẫu nhiên
                randomQues = _dbContext.Question
                                       .AsEnumerable()
                                       .OrderBy(x => Guid.NewGuid())
                                       .Take(total)
                                       .ToList();

                // Lưu lại danh sách ID câu hỏi vào TempData để dùng cho các request tiếp theo
                TempData["QuestionIds"] = string.Join(",", randomQues.Select(q => q.QuestionId));
            }

            var questionOptions = randomQues.Select((que, index) => new SelectListItem
            {
                Value = que.QuestionId.ToString(),
                Text = $"Câu {index + 1}: {que.QContent}"
            }).ToList();

            // --- 2. Lấy thông tin người dùng ---
            var loginName = User.Identity?.Name;
            if (string.IsNullOrEmpty(loginName))
            {
                loginName = "anonymous";
            }
            var ids1 = new List<int>();
            var user = _dbContext.UserAccount.FirstOrDefault(u => u.UserName == loginName);
            var fullName = user?.FullName ?? loginName;

            // --- 3. Lấy thông tin môn học ---
            var subjectName = _dbContext.Subject01
                                        .Where(x => x.SubjectId == subjectid)
                                        .Select(x => x.SubjectName)
                                        .FirstOrDefault();

            // --- 4. Lấy câu hỏi chi tiết nếu có selectedId ---
            Question? selectedQuestion = null;

            if (selectedIndex.HasValue && selectedIndex >= 0 && selectedIndex < questionOptions.Count)
            {
                // Nếu có selectedIndex hợp lệ, lấy câu hỏi tương ứng
                var questionId = int.Parse(questionOptions[selectedIndex.Value].Value);
                selectedQuestion = randomQues.FirstOrDefault(q => q.QuestionId == questionId);
            }

            // xử lý chuyển câu 
            // --- 3. Xác định chỉ mục đang chọn ---
            if (selectedIndex == null || selectedIndex < 0 || selectedIndex >= questionOptions.Count)
            {
                selectedIndex = 0; // Nếu không có chỉ mục hợp lệ, đặt về 0
            }

            // Nếu selectedIndex không hợp lệ, đặt về 0
            if (selectedIndex < 0) selectedIndex = 0;
            if (selectedIndex >= questionOptions.Count) selectedIndex = questionOptions.Count - 1;
            // Lấy câu hỏi tương ứng với selectedIndex
            selectedQuestion = randomQues.ElementAtOrDefault(selectedIndex.Value);
            // giới hạn thời gian thi 
            var time = 0;
            // --- 5. Tạo và trả về ViewModel ---
            var model = new HomeTest
            {
                SelectedIndex = selectedIndex.Value, // Chỉ mục câu hỏi hiện tại
                Subject01 = new Subject01
                {
                    SubjectId = subjectid,
                    SubjectName = subjectName
                },
                TotalQuestion = total,
                Account = new UserAccount
                {
                    FullName = fullName
                },
                Question = selectedQuestion, // Câu hỏi chi tiết được hiển thị
                QuestionOptions = questionOptions, // Danh sách cho ListBox
                SelectedQuestionId = selectedIndex // Gán ID câu hỏi đang được chọn/hiển thị
            };

            return View(model);
        }
        public IActionResult TdsestStart(string subjectid, int total, int? selectedId)
        {
            // --- 1. Chuẩn bị danh sách các câu hỏi cho ListBox (QuestionOptions) ---
            var questionIdsStr = HttpContext.Session.GetString("QuestionIds");
            List<int> ids;

            if (string.IsNullOrEmpty(questionIdsStr))
            {
                // Lần đầu load: random danh sách
                ids = _dbContext.Question
                    .OrderBy(q => Guid.NewGuid())
                    .Take(total)
                    .Select(q => q.QuestionId)
                    .ToList();

                // Lưu vào Session
                HttpContext.Session.SetString("QuestionIds", string.Join(",", ids));
            }
            else
            {
                // Lấy lại danh sách từ Session
                ids = questionIdsStr.Split(',').Select(int.Parse).ToList();
            }
            List<Question> randomQues = new List<Question>();

            var questionOptions = randomQues.Select((que, index) => new SelectListItem
            {
                Value = que.QuestionId.ToString(),
                Text = $"Câu {index + 1}: {que.QContent}"
            }).ToList();

            // --- 2. Lấy thông tin người dùng ---
            var loginName = User.Identity?.Name;
            if (string.IsNullOrEmpty(loginName))
            {
                loginName = "anonymous";
            }
            var ids1 = new List<int>();
            var user = _dbContext.UserAccount.FirstOrDefault(u => u.UserName == loginName);
            var fullName = user?.FullName ?? loginName;

            // --- 3. Lấy thông tin môn học ---
            var subjectName = _dbContext.Subject01
                                        .Where(x => x.SubjectId == subjectid)
                                        .Select(x => x.SubjectName)
                                        .FirstOrDefault();

            // --- 4. Lấy câu hỏi chi tiết nếu có selectedId ---
            Question? selectedQuestion1 = null;

            int selectedIndex = 0;// tạo chỉ mục cho từng câu hỏi
                                  // --- 3. Xác định chỉ mục đang chọn ---
            int index = selectedId ?? 0;
            if (index < 0) index = 0;
            if (index >= questionOptions.Count) index = questionOptions.Count - 1;

            var selectedQuestion = questionOptions[index];
            // --- 5. Tạo và trả về ViewModel ---
            var model = new HomeTest
            {
                SelectedIndex = selectedIndex, // Chỉ mục câu hỏi hiện tại
                Subject01 = new Subject01
                {
                    SubjectId = subjectid,
                    SubjectName = subjectName
                },
                TotalQuestion = total,
                Account = new UserAccount
                {
                    FullName = fullName
                },
                Question = selectedQuestion1, // Câu hỏi chi tiết được hiển thị
                QuestionOptions = questionOptions, // Danh sách cho ListBox
                SelectedQuestionId = selectedId // Gán ID câu hỏi đang được chọn/hiển thị
            };

            return View(model);
        }
        [HttpPost,ActionName("TestStart")]
        public IActionResult ConFirm(string subjectid, int total, int? selectedIndex)
        {
            // Lấy danh sách ID từ TempData
            var idsString = TempData["QuestionIds"] as string;

            if (string.IsNullOrEmpty(idsString))
            {
                return RedirectToAction("TestStart", new { subjectid, total });
            }

            // Giữ TempData sau khi đọc
            TempData.Keep("QuestionIds");

            var idList = idsString.Split(',').Select(int.Parse).ToList();

            // Lấy danh sách câu hỏi theo ID
            var questions = _dbContext.Question
                .Where(q => idList.Contains(q.QuestionId))
                .ToList();

            var options = questions.Select((que, index) => new SelectListItem
            {
                Value = que.QuestionId.ToString(),
                Text = $"Câu {index + 1}: {que.QContent}"
            }).ToList();

            Question selectedQuestion = null;
            if (selectedIndex.HasValue && selectedIndex >= 0 && selectedIndex < questions.Count)
            {
                selectedQuestion = questions[selectedIndex.Value];
            }
            var model = new HomeTest
            {
                Subject01 = new Subject01
                {
                    SubjectId = subjectid
                },
                TotalQuestion = total,
                QuestionOptions = options,
                //Question = _selectedIndex < 0 ? null : questions.ElementAtOrDefault(_selectedIndex),
            };

            return View(model);
        }

    }
}
