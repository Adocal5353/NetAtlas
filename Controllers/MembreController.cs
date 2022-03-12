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
            return bd.Admin.Any(a => a.Id == id);
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
        public async Task<IActionResult> Login(string? password, string? email)
        {
            var member = await bd.Membre.FirstOrDefaultAsync(m => m.Email == email);
            var admin = await bd.Admin.FirstOrDefaultAsync(ad => ad.Email == email);
            var moderateur = await bd.Moderateur
                .FirstOrDefaultAsync(mo => mo.Email == email);

            if (member != null)
            {
                if (member.Password == password)
                {
                    member.isLogged = 1;
                    try
                    {

                        bd.Update(member);
                        await bd.SaveChangesAsync();
                        HttpContext.Session.SetString("UserSession", JsonSerializer.Serialize(member));
                        HttpContext.Session.SetString("UserType", "membre");
                        return RedirectToAction("Index", "Amities");

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
                if (admin != null)
                {
                    if (admin.Password == password)
                    {
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
                            else { throw; }
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
                                return RedirectToAction("Index", "Moderateurs");

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

        public ActionResult Deconnexion()
        {
            HttpContext.Session.Remove("userSession");
            HttpContext.Session.Remove("UserType");
            return RedirectToAction("Index", "Home");
            
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
                var user = GetMembre();

                ViewBag.Membre = user.Nom + " " + user.Prenom;

                var q = await bd.Membre.Where(m => m.Id != user.Id).ToListAsync();
                var q2 = bd.Amitie.Include(a => a.Receiver).Where(a => a.IdSender == user.Id);
                var q3 = bd.Amitie.Include(a => a.Sender).Where(a => a.IdReceiver == user.Id);

                if (q2 != null || q3 != null)
                {




                    foreach (var item2 in q2)
                    {


                        q.Remove(item2.Receiver);

                    }
                    foreach (var item2 in q3)
                    {
                        q.Remove(item2.Sender);
                    }


                    return View(q);
                }
                else
                {
                    return View(q);
                }


            }


        }
        public async Task<IActionResult> RetirerAmi()
        {
            var type = HttpContext.Session.GetString("UserType");
            if (type is not "membre")
            {
                return RedirectToAction("Login", "Membre");
            }
            else
            {
                var user = GetMembre();
            
                ViewBag.Membre = user.Nom + " " + user.Prenom;

                var q = await bd.Membre.Where(m => m.Id != user.Id).ToListAsync();
                var q2 = bd.Amitie.Include(a => a.Receiver).Where(a => a.IdSender == user.Id && a.Statut == 1);
                var q3 = bd.Amitie.Include(a => a.Sender).Where(a => a.IdReceiver == user.Id && a.Statut==1);

                if (q2 != null || q3 != null)
                {
                    var listAmi = new List<Membre>();

                    foreach (var item2 in q2)
                    {
                        if(!listAmi.Contains(item2.Receiver))
                        {
                            listAmi.Add(item2.Receiver);
                        }

                    }
                    foreach (var item2 in q3)
                    {
                        if (!listAmi.Contains(item2.Sender))
                        {
                            listAmi.Add(item2.Sender);
                        }
                    }
                    ViewBag.checkEnvoi = false;

                    return View(listAmi);
                }
                else
                {
                    ViewBag.checkEnvoi = false;
                    return View();
                }
            }
        }



        [HttpPost, ActionName("RetirerAmi")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ValiderRetrait(int id)
        {
            var type = HttpContext.Session.GetString("UserType");
            if (type is not "membre")
            {
                return RedirectToAction("Login", "Membre");
            }
            else
            {
                var user = GetMembre();
                var amitie = await bd.Amitie.FirstOrDefaultAsync(a=> (a.IdSender==id && a.IdReceiver==user.Id) || (a.IdReceiver == id && a.IdSender == user.Id));
                if(amitie is null)
                {
                    return NotFound();
                }
               
                bd.Amitie.Remove(amitie);
                
                await bd.SaveChangesAsync();

                ViewBag.Membre = user.Nom + " " + user.Prenom;

                var q = await bd.Membre.Where(m => m.Id != user.Id).ToListAsync();
                var q2 = bd.Amitie.Include(a => a.Receiver).Where(a => a.IdSender == user.Id && a.Statut == 1);
                var q3 = bd.Amitie.Include(a => a.Sender).Where(a => a.IdReceiver == user.Id && a.Statut == 1);

                
                    var listAmi = new List<Membre>();

                    foreach (var item2 in q2)
                    {
                        if (!listAmi.Contains(item2.Receiver))
                        {
                            listAmi.Add(item2.Receiver);
                        }

                    }
                    foreach (var item2 in q3)
                    {
                        if (!listAmi.Contains(item2.Sender))
                        {
                            listAmi.Add(item2.Sender);
                        }
                    }
                    ViewBag.checkEnvoi =true;

                    return View(listAmi);
                
            }
        }

        public async Task<IActionResult> MesPub()
        {
            var type = HttpContext.Session.GetString("UserType");
            if (type is not "membre")
            {
                return RedirectToAction("Login", "Membre");
            }
            else
            {
                var user = GetMembre();
                ViewBag.Membre = user.Nom + " " + user.Prenom;
                

                var q1 = await bd.Publication.Include(p => p.Menber).ToListAsync();
                var mylist = new List<Dictionary<string, object>>();

                var pub = new List<Publication>();
                foreach (var item in q1)
                {
   

                    if (item.Menber.Id == user.Id)
                    {
                        pub.Add(item);
                    }


                }



                foreach (var item in pub)
                {
                    if (item.etat == false)
                    {
                        var dico = new Dictionary<string, object>();
                        var res = await bd.Lien.AnyAsync(r => r.IdPublication == item.Id);
                        var res2 = await bd.Message.AnyAsync(r => r.IdPublication == item.Id);
                        var res3 = await bd.PhotoVideo.AnyAsync(r => r.IdPublication == item.Id);
                        if (res is true)
                        {
                            dico.Add("publication", item);

                            dico.Add("ressource", await bd.Lien.FirstAsync(r => r.IdPublication == item.Id));
                        }
                        else if (res2 is true)
                        {
                            dico.Add("publication", item);

                            dico.Add("ressource", await bd.Message.FirstAsync(r => r.IdPublication == item.Id));
                        }
                        else if (res3 is true)
                        {
                            dico.Add("publication", item);

                            dico.Add("ressource", await bd.PhotoVideo.FirstAsync(r => r.IdPublication == item.Id));
                        }

                        if (res == true || res2 == true || res3 == true)
                            mylist.Add(dico);
                    }
                }
                ViewBag.check = false;
                ViewBag.ListPub = mylist;
                return View();

            }

        }


        [HttpPost, ActionName("MesPub")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            

                if (id == null)
                    return NotFound();
                var pu = await bd.Publication.FindAsync(id);
                ViewBag.check = true;
                if (pu == null)
                    return NotFound();
                bd.Publication.Remove(pu);
                await bd.SaveChangesAsync();


            var type = HttpContext.Session.GetString("UserType");
            if (type is not "membre")
            {
                return RedirectToAction("Login", "Membre");
            }
            else
            {
                var user = GetMembre();
                ViewBag.Membre = user.Nom + " " + user.Prenom;


                var q1 = await bd.Publication.Include(p => p.Menber).ToListAsync();
                var mylist = new List<Dictionary<string, object>>();

                var pub = new List<Publication>();
                foreach (var item in q1)
                {


                    if (item.Menber.Id == user.Id)
                    {
                        pub.Add(item);
                    }


                }



                foreach (var item in pub)
                {
                    if (item.etat == false)
                    {
                        var dico = new Dictionary<string, object>();
                        var res = await bd.Lien.AnyAsync(r => r.IdPublication == item.Id);
                        var res2 = await bd.Message.AnyAsync(r => r.IdPublication == item.Id);
                        var res3 = await bd.PhotoVideo.AnyAsync(r => r.IdPublication == item.Id);
                        if (res is true)
                        {
                            dico.Add("publication", item);

                            dico.Add("ressource", await bd.Lien.FirstAsync(r => r.IdPublication == item.Id));
                        }
                        else if (res2 is true)
                        {
                            dico.Add("publication", item);

                            dico.Add("ressource", await bd.Message.FirstAsync(r => r.IdPublication == item.Id));
                        }
                        else if (res3 is true)
                        {
                            dico.Add("publication", item);

                            dico.Add("ressource", await bd.PhotoVideo.FirstAsync(r => r.IdPublication == item.Id));
                        }

                        if (res == true || res2 == true || res3 == true)
                            mylist.Add(dico);
                    }
                }
                ViewBag.check = true;
                ViewBag.ListPub = mylist;
                return View(await bd.Publication.Where(p => p.etat == true).ToListAsync());
            }
        }

    }
}
