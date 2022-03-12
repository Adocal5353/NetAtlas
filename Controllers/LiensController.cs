#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NetAtlas.Data;
using NetAtlas.Models;

namespace NetAtlas.Controllers
{
    public class LiensController : Controller
    {
        private readonly NetAtlasContext _context;

        public LiensController(NetAtlasContext context)
        {
            _context = context;
        }

        public Membre? GetMembre()
        {
            var value = HttpContext.Session.GetString("UserSession");
            return value == null ? null : JsonSerializer.Deserialize<Membre>(value);
        }

        // GET: Liens
        public async Task<IActionResult> Index()
        {
            var netAtlasContext = _context.Lien.Include(l => l.Publication);
            return View(await netAtlasContext.ToListAsync());
        }

        // GET: Liens/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lien = await _context.Lien
                .Include(l => l.Publication)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (lien == null)
            {
                return NotFound();
            }

            return View(lien);
        }

        // GET: Liens/Create
        public IActionResult Create()
        {
            var type = HttpContext.Session.GetString("UserType");
            if (type is not "membre")
            {
                return Unauthorized();
            }
            var user = GetMembre();
            ViewBag.Membre = user.Nom + " " + user.Prenom;
            return View();
        }

        // POST: Liens/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( string Url, string nomRessource)
        {
            var type = HttpContext.Session.GetString("UserType");
            if (type is not "membre")
            {
                return Unauthorized();
            }
            var user = GetMembre();
            Publication p = new Publication();
            p.DatePublication = DateTime.Now;
            p.IdMemdre = user.Id;
            _context.Publication.Add(p);
            await _context.SaveChangesAsync();
            var Lien = new Lien();
            Lien.IdPublication = p.Id;
            Lien.Url = Url;
            Lien.nomRessource = nomRessource;
            ViewBag.Membre = user.Nom + " " + user.Prenom;
            _context.Lien.Add(Lien);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
           
        }

        // GET: Liens/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lien = await _context.Lien.FindAsync(id);
            if (lien == null)
            {
                return NotFound();
            }
            return View(lien);
        }

        // POST: Liens/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Url,Id,IdPublication,nomRessource")] Lien lien)
        {
            if (id != lien.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lien);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LienExists(lien.Id))
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
            ViewData["IdPublication"] = new SelectList(_context.Publication, "Id", "Id", lien.IdPublication);
            return View(lien);
        }

        // GET: Liens/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lien = await _context.Lien
                .Include(l => l.Publication)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (lien == null)
            {
                return NotFound();
            }

            return View(lien);
        }

        // POST: Liens/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var lien = await _context.Lien.FindAsync(id);
            _context.Lien.Remove(lien);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LienExists(int id)
        {
            return _context.Lien.Any(e => e.Id == id);
        }
    }
}
