#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NetAtlas.Data;
using NetAtlas.Models;

namespace NetAtlas.Controllers
{
    public class RegistersController : Controller
    {
        private readonly NetAtlasContext _context;

        public RegistersController(NetAtlasContext context)
        {
            _context = context;
        }

        // GET: Registers
        public async Task<IActionResult> Index()
        {
            return View(await _context.Register.ToListAsync());
        }

        // GET: Registers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var register = await _context.Register
                .FirstOrDefaultAsync(m => m.Id == id);
            if (register == null)
            {
                return NotFound();
            }

            return View(register);
        }

        // GET: Registers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Registers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nom,Prenom,Email,Password,ConfirmPassword")] Register register)
        {
            int test=0;
            if ((_context.Register.Any(r => r.Email == register.Email)) || (_context.Membre.Any(r => r.Email == register.Email)))
            {
                ViewBag.MessageEmail = "Cet email est déjà utilisé";
                test++;
                return View();
            }
            if(register.ConfirmPassword==register.Password)
            {
                if (ModelState.IsValid && test==0)
                {
                    _context.Add(register);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                ViewBag.MessagePassword = "Les 2 mots de passe ne sont pas les mêmes";
                return View();
            }
            
            return View(register);
        }

        // GET: Registers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var register = await _context.Register.FindAsync(id);
            if (register == null)
            {
                return NotFound();
            }
            return View(register);
        }

        // POST: Registers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nom,Prenom,Email,Password,ConfirmPassword")] Register register)
        {
            if (id != register.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(register);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RegisterExists(register.Id))
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
            return View(register);
        }

        // GET: Registers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var register = await _context.Register
                .FirstOrDefaultAsync(m => m.Id == id);
            if (register == null)
            {
                return NotFound();
            }

            return View(register);
        }

        // POST: Registers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var register = await _context.Register.FindAsync(id);
            _context.Register.Remove(register);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RegisterExists(int id)
        {
            return _context.Register.Any(e => e.Id == id);
        }
    }
}
