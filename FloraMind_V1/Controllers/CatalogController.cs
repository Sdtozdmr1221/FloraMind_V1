using Microsoft.AspNetCore.Mvc;
using FloraMind_V1.Models;
using FloraMind_V1.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace FloraMind_V1.Controllers
{
    public class CatalogController : Controller
    {
        private readonly FloraMindDbContext _context;

        public CatalogController(FloraMindDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        // --- ARAMA MOTORU KISMI (ShowCatalog) ---
        public async Task<IActionResult> ShowCatalog(string searchString)
        {
            // 1. Veritabanı sorgusunu hazırla (Henüz veriyi çekme)
            var plantsQuery = _context.Plants
                  .Include(p => p.Contents)
                  .AsQueryable();

            // 2. Eğer arama kutusu doluysa filtrele
            if (!string.IsNullOrEmpty(searchString))
            {
                // 'Species' (Tür) sütununda aranan kelimeyi bul
                plantsQuery = plantsQuery.Where(p => p.Species.Contains(searchString));

                // Aranan kelimeyi kutuda kalsın diye geri gönder
                ViewData["CurrentFilter"] = searchString;
            }

            // 3. Sonuçları getir ve View'a gönder
            var result = await plantsQuery.ToListAsync();
            return View(result);
        }

        // --- DETAY SAYFASI ---
        public IActionResult Details(int id)
        {
            var plant = _context.Plants.FirstOrDefault(p => p.PlantID == id);

            if (plant == null)
            {
                return RedirectToAction("ShowCatalog");
            }
            return View(plant);
        }

        // --- PROFİL DETAYLARI ---
        public async Task<IActionResult> UserProfileDetails(int id)
        {
            var UserDetails = await _context.Users
                .FirstOrDefaultAsync(u => u.UserID == id);
            return View();
        }

        public async Task<IActionResult> ChangeUserName()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddToMyPlants(int id)
        {
            // Katalogdan bitkiyi bul
            var catalogPlant = await _context.Plants.FindAsync(id);
            if (catalogPlant == null)
            {
                return NotFound();
            }

            //Mevcut kullanıcının ID'sini al
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Name == User.Identity.Name);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            //UserPlant nesnesini oluştur
            var newUserPlant = new UserPlant
            {
                PlantID = catalogPlant.PlantID,
                UserID = user.UserID,
                WateringIntervalHours = catalogPlant.DefaultWateringIntervalHours,
                DateAdopted = DateTime.Now
            };

            // Sulama hesaplamasını yap
            newUserPlant.PerformWatering();

            // Veritabanına ekle
            _context.UserPlants.Add(newUserPlant);
            await _context.SaveChangesAsync();

            TempData["Message"] = $"{catalogPlant.Name} başarıyla bitkilerime eklendi!";

            return RedirectToAction("Index", "UserPlants");
        }
    }
}