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

           
            return 1;
        }

    
        public async Task<IActionResult> Index()
        {
            var userId = GetLoggedInUserId();

            var userPlants = await _context.UserPlants
                                           .Where(up => up.UserID == userId)
                                           .Include(up => up.Plant) 
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

            
            var existingUserPlant = await _context.UserPlants
                                                  .AnyAsync(up => up.UserID == userId && up.PlantID == plantId);
            if (existingUserPlant)
            {
                TempData["Message"] = $"{catalogPlant.Name} zaten koleksiyonunuzda mevcut.";
                return RedirectToAction(nameof(Index));
            }

            
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Water(int userPlantId)
        {
            var userId = GetLoggedInUserId();

          
            var userPlant = await _context.UserPlants
                .FirstOrDefaultAsync(up => up.UserPlantID == userPlantId && up.UserID == userId);

            if (userPlant == null)
            {
                return NotFound("Sulama işlemi için uygun bir bitki kaydı bulunamadı.");
            }

            // LastWatered alanını güncelle
            userPlant.LastWatered = DateTime.UtcNow;

            
            await _context.SaveChangesAsync();

            TempData["Message"] = "Bitkiniz başarıyla sulandı!";
            return RedirectToAction(nameof(Index));
        }
    }
}