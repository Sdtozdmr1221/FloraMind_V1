using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using FloraMind_V1.Data;
using FloraMind_V1.Models;
using System.Text;

namespace FloraMind_V1.Controllers
{
    public class AccountController : Controller //Kullanıcı işlemleri için kontrolcü
    {

        private readonly FloraMindDbContext _context; //veritabanına erişim

        public AccountController(FloraMindDbContext context)
        {
            _context = context;
        }


        public IActionResult Index()
        {
            return View();
        }



        public IActionResult Register()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model) //Account/Register sayfasından gelen isteği karşılar
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            if (await _context.Users.AnyAsync(u => u.Email == model.Email)) //Email veritabanında kayıtlı mı?
            {
                ModelState.AddModelError("Email", "Email zaten kayıtlı");
                return View(model);
            }


            string passwordHash = HashPassword(model.Password);

            var user = new User
            {
                Name = model.UserName,
                Email = model.Email,
                PasswordHash = passwordHash,
            };


            _context.Users.Add(user);

            await _context.SaveChangesAsync();

            // oturum açma için depolama
            HttpContext.Session.SetString("UserEmail", user.Email);
            HttpContext.Session.SetString("UserName", user.Name);

            return RedirectToAction("Index", "Home");

        }

        private string HashPassword(string password) //kullanıcı şifre koruma metodu
        {
            using var sha256 = SHA256.Create();
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }


        public IActionResult Login()
        {
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _context.Users
    .FirstOrDefaultAsync(u => u.Email == model.Email);

            if (user == null || !VerifyPassword(model.Password, user.PasswordHash))
            {
                ModelState.AddModelError(string.Empty, "Geçersiz email veya şifre");
                return View(model);
            }


            HttpContext.Session.SetString("UserEmail", user.Email);
            HttpContext.Session.SetString("UserName", user.Name);

            return RedirectToAction("Index", "Home");
        }




        private bool VerifyPassword(string EnteredPassword, string storedHashPassword)
        {
            return HashPassword(EnteredPassword) == storedHashPassword;
        }

        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); //  oturumu kapat
            return RedirectToAction("Index", "Home");
        }

    }
}

