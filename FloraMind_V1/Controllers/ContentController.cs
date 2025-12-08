using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FloraMind_V1.Data;
using FloraMind_V1.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims; // User ID almak için

namespace FloraMind_V1.Controllers
{
    // Yalnızca 'Admin' rolüne sahip kullanıcıların erişimine izin verir.
    //[Authorize(Roles = "Admin")]
    public class ContentController : Controller
    {
        private readonly FloraMindDbContext _context;

        public ContentController(FloraMindDbContext context)
        {
            _context = context;
        }

        // Oturum açmış kullanıcının ID'sini (Admin) alır.
        private int GetLoggedInUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim != null && int.TryParse(userIdClaim, out int userId))
            {
                return userId;
            }
            // Identity sistemi kuruluysa bu durum genelde olmaz. Test amaçlı varsayım kaldırıldı.
            throw new UnauthorizedAccessException("Oturum açmış kullanıcı ID'si bulunamadı veya geçerli değil.");
        }


        // GET: Content (Tüm içerikleri listele)
        public async Task<IActionResult> Index()
        {
            // İlgili Plant ve User verilerini Eager Loading ile yükle
            var contents = await _context.Contents
                .Include(c => c.Plant)
                .Include(c => c.User)
                .ToListAsync();

            return View(contents);
        }

        // GET: Content/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var content = await _context.Contents
                .Include(c => c.Plant)
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.ContentID == id);

            if (content == null)
            {
                return NotFound();
            }

            return View(content);
        }

        // GET: Content/Create
        public async Task<IActionResult> Create()
        {
            // Sadece Plant listesi View'a gönderilir (UserID Admin tarafından otomatik atanacak)
            ViewData["PlantID"] = new SelectList(await _context.Plants.ToListAsync(), "PlantID", "Name");
            return View();
        }

        // POST: Content/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        // Sadece Title, Body ve PlantID alınır. ContentID, UserID, DateCreated Bind'den çıkarıldı.
        public async Task<IActionResult> Create([Bind("Title,Body,PlantID")] Content content)
        {
            if (ModelState.IsValid)
            {
                // Güvenlik ve Tutarlılık İçin Atamalar
                content.UserID = 1;
                ModelState.Remove("UserID");
                content.DateCreated = DateTime.UtcNow;
                _context.Add(content);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Hata varsa listeyi tekrar yükle
            ViewData["PlantID"] = new SelectList(await _context.Plants.ToListAsync(), "PlantID", "Name", content.PlantID);
            return View(content);
        }

        // GET: Content/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var content = await _context.Contents.FindAsync(id);
            if (content == null)
            {
                return NotFound();
            }
            // Sadece Plant listesi View'a gönderilir
            ViewData["PlantID"] = new SelectList(await _context.Plants.ToListAsync(), "PlantID", "Name", content.PlantID);
            return View(content);
        }

        // POST: Content/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        // Sadece Title, Body ve PlantID alınır. UserID ve DateCreated Bind'den çıkarıldı.
        public async Task<IActionResult> Edit(int id, [Bind("ContentID,Title,Body,PlantID")] Content content)
        {
            if (id != content.ContentID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // Güncellenmeyecek alanları (UserID, DateCreated) korumak için
                var contentToUpdate = await _context.Contents.AsNoTracking().FirstOrDefaultAsync(c => c.ContentID == id);

                if (contentToUpdate == null) return NotFound();

                // View'dan gelmeyen ancak korumamız gereken değerleri ata
                content.UserID = contentToUpdate.UserID;
                content.DateCreated = contentToUpdate.DateCreated;

                try
                {
                    _context.Update(content);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContentExists(content.ContentID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            // Hata varsa listeyi tekrar yükle
            ViewData["PlantID"] = new SelectList(await _context.Plants.ToListAsync(), "PlantID", "Name", content.PlantID);
            return View(content);
        }

        // GET: Content/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            // Details ile aynı mantık: ilişkili verileri yükle
            if (id == null)
            {
                return NotFound();
            }

            var content = await _context.Contents
                .Include(c => c.Plant)
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.ContentID == id);

            if (content == null)
            {
                return NotFound();
            }

            return View(content);
        }

        // POST: Content/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var content = await _context.Contents.FindAsync(id);
            if (content != null)
            {
                _context.Contents.Remove(content);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContentExists(int id)
        {
            return _context.Contents.Any(e => e.ContentID == id);
        }
    }
}