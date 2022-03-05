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
    public class AmitiesController : Controller
    {
        private readonly NetAtlasContext _context;

        public AmitiesController(NetAtlasContext context)
        {
            _context = context;
        }
        public Membre? GetMembre()
        {
            var value = HttpContext.Session.GetString("UserSession");
            return value == null ? null : JsonSerializer.Deserialize<Membre>(value);
        }

        // GET: Amities
        public async Task<IActionResult> Index()
        {
            var type = HttpContext.Session.GetString("UserType");
            if (type is not "membre")
            {
                return Unauthorized();
            }
            var user = GetMembre();
            var netAtlasContext = _context.Amitie.Include(a => a.Receiver).Where(a=>a.IdSender==user.Id && a.Statut==0);
            return View(await netAtlasContext.ToListAsync());
        }

        // GET: Amities/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var amitie = await _context.Amitie
                .Include(a => a.Receiver)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (amitie == null)
            {
                return NotFound();
            }

            return View(amitie);
        }

        

        // POST: Amities/Create/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int? id)
        {
            var receiver = await _context.Membre.FindAsync(id);
            var type = HttpContext.Session.GetString("UserType");
            if (type is not "membre")
            {
                return RedirectToAction("Login","Membre");
            }
            else
            {
                var user = GetMembre();
                var amitie = new Amitie();
                amitie.IdSender = user.Id;
                amitie.Receiver = receiver;
                amitie.IdReceiver = receiver.Id;
                amitie.Statut = 0;
                try
                {
                    _context.Add(amitie);
                    await _context.SaveChangesAsync();
                }
                catch
                {
                    throw;
                }

                return RedirectToAction("ListeMembre", "Membre");
            }
        }

        // POST: Amities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id)
        {
            var amitie = _context.Amitie.Find(id);

            if (amitie is Amitie)
            {
                amitie.Statut=1;
                try
                {
                    _context.Update(amitie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AmitieExists(amitie.Id))
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
            return RedirectToAction(nameof(Index));
        }

        // GET: Amities/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var amitie = await _context.Amitie
                .Include(a => a.Receiver)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (amitie == null)
            {
                return NotFound();
            }

            return View(amitie);
        }

        // POST: Amities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var amitie = await _context.Amitie.FindAsync(id);
            _context.Amitie.Remove(amitie);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AmitieExists(int id)
        {
            return _context.Amitie.Any(e => e.Id == id);
        }
    }
}
