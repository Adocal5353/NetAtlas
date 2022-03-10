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
    public class ModerateursController : Controller
    {
        private readonly NetAtlasContext _context;

        public ModerateursController(NetAtlasContext context)
        {
            _context = context;
        }

        public Moderateur? GetModerateur()
        {
            var value = HttpContext.Session.GetString("UserSession");
            return value == null ? null : JsonSerializer.Deserialize<Moderateur>(value);
        }

        // GET: Moderateurs
        public async Task<IActionResult> Index()
        {
            var type = HttpContext.Session.GetString("UserType");
            if (type is not "moderateur")
            {
                return RedirectToAction("Login", "Membre");
            }
            else
            {
                var user = GetModerateur();
                ViewBag.Moderateur = user.Nom + " " + user.Prenom;
                var q1 = await _context.Publication.ToListAsync();
                var mylist = new List<Dictionary<string, object>>();

                foreach (var item in q1)
                {
                    var dico = new Dictionary<string, object>();
                    dico.Add("publication", item);
                    var res = await _context.Lien.AnyAsync(r => r.IdPublication == item.Id);
                    var res2 = await _context.Message.AnyAsync(r => r.IdPublication == item.Id);
                    var res3 = await _context.PhotoVideo.AnyAsync(r => r.IdPublication == item.Id);
                    if (res is true)
                    {
                        dico.Add("ressource", await _context.Lien.FirstAsync(r => r.IdPublication == item.Id));
                    }
                    else if (res2 is true)
                    {
                        dico.Add("ressource", await _context.Message.FirstAsync(r => r.IdPublication == item.Id));
                    }
                    else if (res3 is true)
                    {
                        dico.Add("ressource", await _context.PhotoVideo.FirstAsync(r => r.IdPublication == item.Id));
                    }

                    if (dico is not null)
                        mylist.Add(dico);
                }
                ViewBag.ListPub = mylist;
                return View();
            }
        }
        // GET: Moderateurs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var moderateur = await _context.Moderateur
                .FirstOrDefaultAsync(m => m.Id == id);
            if (moderateur == null)
            {
                return NotFound();
            }

            return View(moderateur);
        }

        // GET: Moderateurs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Moderateurs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Password,Nom,Prenom,Email,isLogged")] Moderateur moderateur)
        {
            if (ModelState.IsValid)
            {
                _context.Add(moderateur);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(moderateur);
        }

        // GET: Moderateurs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var moderateur = await _context.Moderateur.FindAsync(id);
            if (moderateur == null)
            {
                return NotFound();
            }
            return View(moderateur);
        }

        // POST: Moderateurs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Password,Nom,Prenom,Email,isLogged")] Moderateur moderateur)
        {
            if (id != moderateur.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(moderateur);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ModerateurExists(moderateur.Id))
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
            return View(moderateur);
        }

        // GET: Moderateurs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var moderateur = await _context.Moderateur
                .FirstOrDefaultAsync(m => m.Id == id);
            if (moderateur == null)
            {
                return NotFound();
            }

            return View(moderateur);
        }

        // POST: Moderateurs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var moderateur = await _context.Moderateur.FindAsync(id);
            _context.Moderateur.Remove(moderateur);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ModerateurExists(int id)
        {
            return _context.Moderateur.Any(e => e.Id == id);
        }

        public async Task<IActionResult> Avertir(int ? id)
        {
            return View();
        }
    }
}
