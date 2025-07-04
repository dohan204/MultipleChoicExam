using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;
using MultipleChoicExam.Models;

namespace MultipleChoicExam.Controllers
{
    public class QuestionController : Controller
    {
        private readonly EFCoreDbContext _dbContext; // dependency injecttion
        public QuestionController(EFCoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult ShowDataQuestion()
        {
            var listQuestion = _dbContext.Question.ToList();
            return View(listQuestion);
        }
        [HttpGet]
        public IActionResult AddQuestion()
        {
            var listSubjects = _dbContext.Subject01.ToList();
            ViewBag.SubjectList = listSubjects
                .Select(s => new SelectListItem
                {
                    Value = s.SubjectId,
                    Text = s.SubjectName
                })
                .ToList();
            return View();
        }
        [HttpPost]
        public IActionResult AddQuestion(Question question) 
        {
            var listSubjects = _dbContext.Subject01.ToList(); // OK
            ViewBag.SubjectList = listSubjects
    .Select(s => new SelectListItem
    {
        Value = s.SubjectId,         // hoặc s.Id nếu đã là string
        Text = s.SubjectName
    })
    .ToList(); // Đây là IEnumerable<SelectListItem>


            if (!ModelState.IsValid) 
            {
                return View("AddQuestion",question);
            }
            
            var addQuestion = new Question
            {
                SubjectId = question.SubjectId,
                QContent = question.QContent,
                OptionA = question.OptionA,
                OptionB = question.OptionB,
                OptionC = question.OptionC,
                OptionD = question.OptionD,
                Answer = question.Answer,
            };
            _dbContext.Question.Add(addQuestion);
            _dbContext.SaveChanges();

            return RedirectToAction("ShowDataQuestion");
        }
        [HttpGet]
        public IActionResult EditQuestion(int id) 
        {
            var listDrop = _dbContext.Subject01.ToList();
            ViewBag.SubjectList = listDrop.Select(s => new SelectListItem { 
                Value = s.SubjectId,
                Text = s.SubjectName
            });
            var question = _dbContext.Question.FirstOrDefault(q => q.QuestionId == id);
            //if (question == null) 
            //{
            //    return NotFound();
            //}
            return View("EditQuestion",question);
        }
        [HttpPost,ActionName("EditQuestion")]
        public IActionResult ComfirmEdit(Question question) 
        {
            var listDrop = _dbContext.Subject01.ToList();
            ViewBag.SubjectList = listDrop.Select(s => new SelectListItem
            {
                Value = s.SubjectId,
                Text = s.SubjectName
            });
            var foundQuestion = _dbContext.Question.FirstOrDefault(q => q.QuestionId == question.QuestionId);
            if (foundQuestion != null) 
            {
                foundQuestion.QuestionId = question.QuestionId;
                foundQuestion.QContent = question.QContent;
                foundQuestion.OptionA = question.OptionA;
                foundQuestion.OptionB = question.OptionB;
                foundQuestion.OptionC = question.OptionC;
                foundQuestion.OptionD = question.OptionD;
                foundQuestion.Answer = question.Answer;
            }
            _dbContext.SaveChanges();
            return RedirectToAction("ShowDataQuestion");
        }
        [HttpGet]
        public IActionResult DeleteQuestion(int id)
        {
            var del = _dbContext.Question.FirstOrDefault(q => q.QuestionId == id);
            if(del == null)
            {
                return NotFound();
            }
            _dbContext.Question.Remove(del);
            _dbContext.SaveChanges();
            return RedirectToAction("ShowDataQuestion");
        }
    }
}
