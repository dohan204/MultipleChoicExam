using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using MultipleChoicExam.Models;

namespace MultipleChoicExam.Controllers
{
    public class ManageUserController : Controller
    {
        private readonly EFCoreDbContext _dbContext;
        public ManageUserController(EFCoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult ManageSubject()
        {
            var subject = new AddSubject { 
                Subject = new Subject01(),
                ListSubject = _dbContext.Subject01.ToList()
            };
            return View(subject);
        }
        [HttpGet]
        public IActionResult AddSubject()
        {
            return View(new Subject01());
        }
        [HttpPost,ActionName("AddSubject")]
        public IActionResult ComfirmAdd(Subject01 sub)
        {
            if(sub == null)
            {
                return View(sub);
            }
            var subject = new Subject01
            {
                SubjectId = sub.SubjectId,
                SubjectName = sub.SubjectName,
                Description = sub.Description,
            };
            if (_dbContext.Subject01.Any(s => s.SubjectId == sub.SubjectId))
            {
                ModelState.AddModelError("", "Môn học này đã tồn tại.");
                return View(sub);
            }
            _dbContext.Subject01.Add(subject);

            _dbContext.SaveChanges();
            return RedirectToAction("ManageSubject");
        }
        [HttpGet]
        public IActionResult EditSubject(string subId)
        {
            var edit = _dbContext.Subject01.FirstOrDefault(s => s.SubjectId == subId);
            if (edit == null)
            {
                return View(new Subject01());
            }
            return View(edit);
        }
        [HttpPost]
        public IActionResult EditSubject(Subject01 mdel)
        {
            var edit = _dbContext.Subject01.FirstOrDefault(s => s.SubjectId == mdel.SubjectId);
            if(edit != null)    
            {
                edit.SubjectName = mdel.SubjectName;
                edit.Description = mdel.Description;
            }
            // lưu lại 
            _dbContext.SaveChanges();
            return RedirectToAction("ManageSubject");
        }
        [HttpPost]
        public IActionResult SearByName(AddSubject add)
        {
            var search = _dbContext.Subject01.FirstOrDefault(s => s.SubjectName == add.Subject.SubjectName);
            if(search == null)
            {
                return View(add);
            }
            return View(search);
        }
        [HttpGet]
        public IActionResult DeleteSubject(string subId) 
        {
            var sub = _dbContext.Subject01.FirstOrDefault(s => s.SubjectId == subId);
            if(sub == null)
            {
                return NotFound();
            }
            return View(sub);
        }
        [HttpPost]
        public IActionResult DeleteSubject(Subject01 model) 
        {
            var delete = _dbContext.Subject01.FirstOrDefault(s => s.SubjectId == model.SubjectId);
            if(delete != null)
            {
                _dbContext.Subject01.Remove(delete);
                _dbContext.SaveChanges();
            }
            return RedirectToAction("ManageSubject");
        }
    }
}
