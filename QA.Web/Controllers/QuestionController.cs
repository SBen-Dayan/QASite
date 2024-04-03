using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using QA.Data;
using QA.Web.Models;

namespace QA.Web.Controllers
{
    public class QuestionController : Controller
    {
        private readonly string _connectionString;

        public QuestionController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("conStr");
        }

        [Authorize]
        public IActionResult AskAQuestion()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult Add(Question question, List<string> tags)
        {
            var repo = new QaRepository(_connectionString);
            question.UserId = GetUserId();
            question.DatePosted = DateTime.Now;
            repo.InsertQuestion(question, tags);
            return Redirect($"/Question/ViewQuestion?id={question.Id}");
        }

        private int GetUserId()
        {
            return new UserRepository(_connectionString).GetUserIdByEmail(User.Identity.Name);
        }

        public IActionResult ViewQuestion(int id)
        {
            var repo = new QaRepository(_connectionString);
            var question = repo.GetQuestion(id);
            if (question == null)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(new QAViewModel { Question = question });
        }

        [Authorize]
        [HttpPost]
        public IActionResult Answer(string text, int questionId)
        {
            var repo = new QaRepository(_connectionString);
            repo.InsertAnswer(new()
            {
                Text = text,
                DatePosted = DateTime.Now,
                QuestionId = questionId,
                UserId = GetUserId()
            });
            return RedirectToAction("ViewQuestion");
        }
    }
}
