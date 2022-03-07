using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetAtlas.Data;
using NetAtlas.Models;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace NetAtlas.Controllers
{
    public class MembreController : Controller
    {
        private readonly NetAtlasContext bd;
        public MembreController(NetAtlasContext context)
        {
            bd = context;
        }
        public Membre? GetMembre()
        {
            var value = HttpContext.Session.GetString("UserSession");
            return value == null ? null : JsonSerializer.Deserialize<Membre>(value);
        }
        private bool MemberExists(int id)
        {
            return bd.Membre.Any(e => e.Id == id);
        }
        private bool AdminExists(int id)
        {
            return bd.Admin.Any(a=>a.Id == id);
        }
        private bool ModerateurExists(int id)
        {
            return bd.Moderateur.Any(m => m.Id == id);
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string? password,string? email)
        {
            var member = await bd.Membre
                .FirstOrDefaultAsync(m => m.Email == email);
            var admin = await bd.Admin
                .FirstOrDefaultAsync(ad=>ad.Email==email);
            var moderateur = await bd.Moderateur
                .FirstOrDefaultAsync(mo => mo.Email == email);

            if (member != null)
            {
                if(member.Password == password)
                {
                    member.isLogged = 1;
                    try
                    {
                    
                        bd.Update(member);
                        await bd.SaveChangesAsync();
                        HttpContext.Session.SetString("UserSession", JsonSerializer.Serialize(member));
                        HttpContext.Session.SetString("UserType", "membre");
                        return RedirectToAction("Index", "Publication");

                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!MemberExists(member.Id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }

                }
            }
            else
            {
                if(admin!=null)
                {
                    if (admin.Password == password) { 
                        admin.isLogged = 1;
                        try
                        {
                            bd.Update(admin);
                            await bd.SaveChangesAsync();
                            HttpContext.Session.SetString("UserSession", JsonSerializer.Serialize(admin));
                            HttpContext.Session.SetString("UserType", "admin");
                            return RedirectToAction("Index", "Admin");
                        }
                        catch
                        {
                            if (!AdminExists(admin.Id))
                            {
                                return NotFound();
                            }
                            else {throw;}
                        }
                   }
                }
                else
                {
                    if (moderateur != null)
                    {
                        if (moderateur.Password == password)
                        {
                            moderateur.isLogged = 1;
                            try
                            {
                                bd.Update(moderateur);
                                await bd.SaveChangesAsync();
                                HttpContext.Session.SetString("UserSession", JsonSerializer.Serialize(moderateur));
                                HttpContext.Session.SetString("UserType", "moderateur");


                            }
                            catch
                            {
                                if (!ModerateurExists(moderateur.Id)) { return NotFound(); }
                                else { throw; }
                            }
                        }
                    }
                    else
                    {
                        ViewBag.NoneValid = "Email ou mot de passe incorrect";

                    }
                }

            }
            ViewBag.NoneValid = "Email ou mot de passe incorrect";

            return View();
        }

        public async Task<IActionResult> ListeMembre()
        {
            var type = HttpContext.Session.GetString("UserType");
            if (type is not "membre")
            {
                return RedirectToAction("Login", "Membre");
            }
            else
            {
                List<Membre> nonAmi = new List<Membre>();

                var user = GetMembre();
                
                var q = await bd.Membre.Where(m=>m.Id!=user.Id).ToListAsync();
                //var q2 = await bd.Amitie.Where(a=>a.Statut==1 && (a.IdSender==user.Id || a.IdReceiver==user.Id)).ToListAsync();
                var q2= bd.Amitie.Include(a => a.Receiver).Where(a => (a.IdSender == user.Id || a.IdReceiver == user.Id) && a.Statut == 1);
                //var q2 = bd.Membre.J
                if (q2 is not null)
                {

                    foreach (var item in q)
                    {
                        foreach (var item2 in q2)
                        {
                            if(item2.Receiver!=item)
                            {
                                nonAmi.Add(item);
                            }
                        }
                    }

                    return View(nonAmi);
                }
                else 
                {
                    return View();
                }
                
                
            }
        }

    }
}
