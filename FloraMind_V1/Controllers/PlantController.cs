using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FloraMind_V1.Data;
using FloraMind_V1.Models;

namespace FloraMind_V1.Controllers
{
    public class PlantController : Controller
    {
        private readonly FloraMindDbContext _context;

        public PlantController(FloraMindDbContext context)
        {
            _context = context;
        }

        // GET: Plant
        public async Task<IActionResult> Index()
        {
            var floraMindDbContext = _context.Plants.Include(p => p.User);
            return View(await floraMindDbContext.ToListAsync());
        }

       

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plant = await _context.Plants
                
                .Include(p => p.Contents)
                .FirstOrDefaultAsync(m => m.PlantID == id);

            if (plant == null)
            {
                return NotFound();
            }

            return View(plant);
        }

        
        public IActionResult Create()
        {
            
            return View();
        }

        
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Species")] Plant plant)
        {
            if (ModelState.IsValid)
            {
                plant.DateAdded = DateTime.UtcNow;
                plant.UserID = null; // Kullanıcı ID'si null olarak ayarlanıyor
                _context.Add(plant);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            return View(plant);
        }

        
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plant = await _context.Plants.FindAsync(id);
            if (plant == null)
            {
                return NotFound();
            }
            
            return View(plant);
        }

        
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PlantID,Name,Species,DateAdded,UserID")] Plant plant)
        {
            if (id != plant.PlantID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(plant);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlantExists(plant.PlantID))
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
            
            return View(plant);
        }

        // GET: Plant/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plant = await _context.Plants
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.PlantID == id);
            if (plant == null)
            {
                return NotFound();
            }

            return View(plant);
        }

        // POST: Plant/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var plant = await _context.Plants.FindAsync(id);
            if (plant != null)
            {
                _context.Plants.Remove(plant);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PlantExists(int id)
        {
            return _context.Plants.Any(e => e.PlantID == id);
        }
    }
}
