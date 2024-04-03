using Microsoft.AspNetCore.Mvc;
using QA.Data;
using QA.Web.Models;
using System.Diagnostics;

namespace QA.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly string _connectionString;

        public HomeController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("conStr");
        }

        public IActionResult Index()
        {
            var repo = new QaRepository(_connectionString);
            return View(new QuestionsViewModel { Questions = repo.GetQuestions()});
        }

        public IActionResult QuestionsByCategory(int id)
        {
            var repo = new QaRepository(_connectionString);
            return View(new QuestionsByCategoryViewModel { Category = repo.GetCategory(id), Questions = repo.GetQuestions(id)});
        }
    }
}