using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FloraMind_V1.Data;
using FloraMind_V1.Models;

namespace FloraMind_V1.Controllers
{
    // Kullanıcının oturum açmış olmasını zorunlu kılar.
    [Authorize]
    public class UserPlantsController : Controller
    {
        private readonly FloraMindDbContext _context;

        public UserPlantsController(FloraMindDbContext context)
        {
            _context = context;
        }

        // Güvenli UserID Çözümleyicisi (Identity sistemi kurulduğunda 'return 1' kaldırılmalıdır!)
        private int GetLoggedInUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim != null && int.TryParse(userIdClaim, out int userId))
            {
                return userId; // Oturum açmış kullanıcının gerçek ID'si
            }

            // DİKKAT: Gerçek uygulamada burası fırlatmalı veya giriş sayfasına yönlendirmeli.
            // Test için sabit ID kullanılıyorsa:
            return 1;
        }

        // --------------------------------------------------------
        // 1. Index: Kullanıcının Koleksiyonunu Görüntüleme
        // --------------------------------------------------------
        public async Task<IActionResult> Index()
        {
            var userId = GetLoggedInUserId();

            var userPlants = await _context.UserPlants
                                           .Where(up => up.UserID == userId)
                                           .Include(up => up.Plant) // Katalog bitkisi detaylarını yükler
                                           .ToListAsync();

            return View(userPlants);
        }

        // --------------------------------------------------------
        // 2. Add: Katalogdan Bitkiyi Koleksiyona Ekleme
        // --------------------------------------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(int plantId)
        {
            var userId = GetLoggedInUserId();

            var catalogPlant = await _context.Plants.FindAsync(plantId);
            if (catalogPlant == null)
            {
                return NotFound("Katalogda bu ID'ye sahip bir bitki bulunamadı.");
            }

            // Çift kaydı önleme kontrolü (İsteğe bağlı, ancak iyi bir pratik)
            var existingUserPlant = await _context.UserPlants
                                                  .AnyAsync(up => up.UserID == userId && up.PlantID == plantId);
            if (existingUserPlant)
            {
                TempData["Message"] = $"{catalogPlant.Name} zaten koleksiyonunuzda mevcut.";
                return RedirectToAction(nameof(Index));
            }

            // Yeni UserPlant kaydını oluşturma (Artık property'ler doğru çalışacak)
            var newUserPlant = new UserPlant
            {
                UserID = userId,
                PlantID = plantId,
                DateAdopted = DateTime.UtcNow,
                LastWatered = DateTime.UtcNow
            };

            _context.UserPlants.Add(newUserPlant);
            await _context.SaveChangesAsync();

            TempData["Message"] = $"{catalogPlant.Name} koleksiyonunuza başarıyla eklendi!";
            return RedirectToAction(nameof(Index));
        }

        // --------------------------------------------------------
        // 3. Water: Bitkiyi Sulama İşlevi (Optimize Edilmiş)
        // --------------------------------------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Water(int userPlantId)
        {
            var userId = GetLoggedInUserId();

            // OPTİMİZASYON: Tek sorguda kaydı bulur ve kullanıcının aitliğini doğrular.
            var userPlant = await _context.UserPlants
                .FirstOrDefaultAsync(up => up.UserPlantID == userPlantId && up.UserID == userId);

            if (userPlant == null)
            {
                // Kayıt bulunamadı VEYA kayıt bu kullanıcıya ait değil.
                return NotFound("Sulama işlemi için uygun bir bitki kaydı bulunamadı.");
            }

            // LastWatered alanını güncelle
            userPlant.LastWatered = DateTime.UtcNow;

            // Context, userPlant objesinin takibini yaptığı için Update gerekmez, 
            // ancak açıkça kullanmak da hata değildir. SaveChanges yeterlidir.
            await _context.SaveChangesAsync();

            TempData["Message"] = "Bitkiniz başarıyla sulandı!";
            return RedirectToAction(nameof(Index));
        }
    }
}