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
    public class MessagesController : Controller
    {
        private readonly NetAtlasContext _context;
        public Membre? GetMembre()
        {
            var value = HttpContext.Session.GetString("UserSession");
            return value == null ? null : JsonSerializer.Deserialize<Membre>(value);
        }

        public MessagesController(NetAtlasContext context)
        {
            _context = context;
        }

        // GET: Messages
        public async Task<IActionResult> Index()
        {
            var netAtlasContext = _context.Message.Include(m => m.Publication);
            return View(await netAtlasContext.ToListAsync());
        }

        // GET: Messages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var message = await _context.Message
                .Include(m => m.Publication)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (message == null)
            {
                return NotFound();
            }

            return View(message);
        }

        // GET: Messages/Create
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

        // POST: Messages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string contenu,string nomRessource)
        {
            var type = HttpContext.Session.GetString("UserType");
            if (type is not "membre")
            {
                return Unauthorized();
            }
            var user = GetMembre();
            ViewBag.Membre = user.Nom + " " + user.Prenom;
            Publication p = new Publication();
            p.DatePublication = DateTime.Now;
            p.IdMemdre = user.Id;
            _context.Publication.Add(p);
            await _context.SaveChangesAsync();
            var message = new Message();
            message.nomRessource = nomRessource;
            message.IdPublication = p.Id;
            message.contenu = contenu;
            _context.Message.Add(message);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        // GET: Messages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var message = await _context.Message.FindAsync(id);
            if (message == null)
            {
                return NotFound();
            }
            ViewData["IdPublication"] = new SelectList(_context.Publication, "Id", "Id", message.IdPublication);
            return View(message);
        }

        // POST: Messages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("contenu,Id,IdPublication,nomRessource")] Message message)
        {
            if (id != message.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(message);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MessageExists(message.Id))
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
            ViewData["IdPublication"] = new SelectList(_context.Publication, "Id", "Id", message.IdPublication);
            return View(message);
        }

        // GET: Messages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var message = await _context.Message
                .Include(m => m.Publication)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (message == null)
            {
                return NotFound();
            }

            return View(message);
        }

        // POST: Messages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var message = await _context.Message.FindAsync(id);
            _context.Message.Remove(message);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MessageExists(int id)
        {
            return _context.Message.Any(e => e.Id == id);
        }
    }
}
