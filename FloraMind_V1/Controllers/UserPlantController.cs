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

        private int GetLoggedInUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim != null && int.TryParse(userIdClaim, out int userId))
            {
                return userId;
            }

<<<<<<< HEAD
           
=======
            
>>>>>>> d8c7b447be227eb7cbc2c982d1bee868abe87254
            return 1;
        }

        
        public async Task<IActionResult> Index()
        {
            var userId = GetLoggedInUserId();

            var userPlants = await _context.UserPlants
                                           .Where(up => up.UserID == userId)
<<<<<<< HEAD
                                           .Include(up => up.Plant) // Bitki detaylarını çek
=======
                                           .Include(up => up.Plant) 
>>>>>>> d8c7b447be227eb7cbc2c982d1bee868abe87254
                                           .ThenInclude(p => p.Contents)
                                           .ToListAsync();

            return View(userPlants);
        }

        
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

            . 
            // Aynı bitkiden birden fazla eklenebilir.

            var newUserPlant = new UserPlant
            {   
                UserID = userId,
                PlantID = plantId,
                DateAdopted = DateTime.UtcNow,
                LastWatered = DateTime.UtcNow,
                Nickname = catalogPlant.Name 
            };

            _context.UserPlants.Add(newUserPlant);
            await _context.SaveChangesAsync();

            TempData["Message"] = $"{catalogPlant.Name} koleksiyonunuza başarıyla eklendi!";
            return RedirectToAction(nameof(Index));
        }

<<<<<<< HEAD
       
=======

>>>>>>> d8c7b447be227eb7cbc2c982d1bee868abe87254
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> WaterPlant(int id)
        {
            var userId = GetLoggedInUserId();

            var userPlant = await _context.UserPlants
                .Include(up => up.Plant)
                .FirstOrDefaultAsync(up => up.UserPlantID == id && up.UserID == userId);

            if (userPlant == null)
            {
                return NotFound("Sulama işlemi için uygun bir bitki kaydı bulunamadı.");
            }
<<<<<<< HEAD
            
=======

>>>>>>> d8c7b447be227eb7cbc2c982d1bee868abe87254
            // Son sulama zamanını şu an olarak ayarla
            userPlant.LastWatered = DateTime.Now;

            double aralik = userPlant.WateringIntervalHours > 0
                          ? userPlant.WateringIntervalHours
                          : (userPlant.Plant != null ? (double)userPlant.Plant.DefaultWateringIntervalHours : 24.0);

            userPlant.NextWateringDate = DateTime.Now.AddHours(aralik);

<<<<<<< HEAD

=======
            
>>>>>>> d8c7b447be227eb7cbc2c982d1bee868abe87254
            // Bitki sulandığı için "E-posta gönderildi" bilgisini sıfırlıyoruz.

            userPlant.IsEmailSent = false;
            // ----------------------------

            _context.Update(userPlant);
            await _context.SaveChangesAsync();

            TempData["Message"] = "Bitkiniz sulandı, geri sayım yeniden başlatıldı!";
            return RedirectToAction(nameof(Index));
        }

<<<<<<< HEAD
        
=======

>>>>>>> d8c7b447be227eb7cbc2c982d1bee868abe87254
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = GetLoggedInUserId();

<<<<<<< HEAD
            // Sadece o kullanıcıya ait bitkiyi bul
=======
            
>>>>>>> d8c7b447be227eb7cbc2c982d1bee868abe87254
            var userPlantToDelete = await _context.UserPlants
                .FirstOrDefaultAsync(up => up.UserPlantID == id && up.UserID == userId);

            if (userPlantToDelete != null)
            {
                _context.UserPlants.Remove(userPlantToDelete);
                await _context.SaveChangesAsync();
                TempData["Message"] = "Bitki bahçenizden silindi.";
            }
            else
            {
                TempData["Error"] = "Bitki bulunamadı veya silinemedi.";
            }

<<<<<<< HEAD
            return RedirectToAction(nameof(Index));
        }

        -
=======

            
            return RedirectToAction(nameof(Index));
        }
          
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = GetLoggedInUserId();

            
            var silinecekBitki = await _context.UserPlants
                .FirstOrDefaultAsync(up => up.UserPlantID == id && up.UserID == userId);

           
            if (silinecekBitki != null)
            {
                _context.UserPlants.Remove(silinecekBitki);
                await _context.SaveChangesAsync();
                TempData["Message"] = "Bitki başarıyla silindi.";
            }

            
            return RedirectToAction(nameof(Index));
        }

        // POST: UserPlants/UpdateNickname/5
>>>>>>> d8c7b447be227eb7cbc2c982d1bee868abe87254
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateNickname(int id, string newNickname)
        {
            var userId = GetLoggedInUserId();

            // Sadece kendi bitkisinin ismini değiştirebilsin
            var userPlant = await _context.UserPlants
                .FirstOrDefaultAsync(up => up.UserPlantID == id && up.UserID == userId);

            if (userPlant == null) return NotFound();

            userPlant.Nickname = newNickname;
            await _context.SaveChangesAsync();

            TempData["Message"] = "Bitki ismi güncellendi.";

            return RedirectToAction(nameof(Index));
        }
    }
}