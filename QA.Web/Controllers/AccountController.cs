using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using QA.Data;
using System.Security.Claims;

namespace QA.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly string _connectionString;

        public AccountController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("conStr");
        }

        public IActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Signup(User user, string password)
        {
            var repo = new UserRepository(_connectionString);
            repo.InsertUser(user, password);
            Login(user.Email);
            return Redirect("/");
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                return RedirectToAction("Login");
            }

            var repo = new UserRepository(_connectionString);
            if (!repo.Verify(email, password))
            {
                return RedirectToAction("Login");
            }

            Login(email);
            return RedirectToAction("index", "home");
        }

        private void Login(string email)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email, email)
            };
            HttpContext.SignInAsync(new ClaimsPrincipal(
                    new ClaimsIdentity(claims, "Cookies", ClaimTypes.Email, "roles"))).Wait();

        }

        public IActionResult Logout()
        {
            HttpContext.SignOutAsync().Wait();
            return RedirectToAction("index", "home");
        }
    }
}
