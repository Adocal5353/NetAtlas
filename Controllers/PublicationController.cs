using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetAtlas.Data;
using NetAtlas.Models;
using System.Text.Json;

namespace NetAtlas.Controllers
{
    public class PublicationController : Controller
    {
        private readonly NetAtlasContext _context;



        public PublicationController(NetAtlasContext context)
        {
            _context = context;
        }
        // GET: PublicationController

        public  Membre? GetMembre()
        {
            var value = HttpContext.Session.GetString("UserSession");
            return value == null ? null : JsonSerializer.Deserialize<Membre>(value);
        }
        public async Task<IActionResult> Index()
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
                var q2 = _context.Amitie.Include(a => a.Receiver).Where(a => a.IdSender == user.Id);
                var q3 = _context.Amitie.Include(a => a.Sender).Where(a => a.IdReceiver == user.Id);
                
                var q1 = await _context.Publication.ToListAsync();
                var mylist = new List<Dictionary<string, object>>();

                var pub = new  List<Publication>();
                foreach (var item in q1)
                {
                    foreach(var item2 in q2)
                    {
                        if(item.Menber==item2.Receiver)
                        {

                            if(!pub.Contains(item))
                            {
                                pub.Add(item);
                            }
                        }
                    }

                    foreach (var item3 in q3)
                    {
                        if (item.Menber == item3.Sender)
                        {

                            if (!pub.Contains(item))
                            {
                                pub.Add(item);
                            }
                        }

                    }
                }


                    foreach (var item in pub)
                {
                    var dico = new Dictionary<string, object>();
                    var res = await _context.Lien.AnyAsync(r => r.IdPublication == item.Id);
                    var res2= await _context.Message.AnyAsync(r => r.IdPublication == item.Id);
                    var res3= await _context.PhotoVideo.AnyAsync(r => r.IdPublication == item.Id);
                    if(res is true)
                    {
                        dico.Add("publication", item);

                        dico.Add("ressource",await _context.Lien.FirstAsync(r=>r.IdPublication==item.Id));
                    } else if(res2 is true)
                    {
                        dico.Add("publication", item);

                        dico.Add("ressource",await _context.Message.FirstAsync(r => r.IdPublication == item.Id));
                    }
                    else if(res3 is true)
                    {
                        dico.Add("publication", item);

                        dico.Add("ressource",await _context.PhotoVideo.FirstAsync(r => r.IdPublication == item.Id));
                    }

                    if(res==true || res2==true || res3==true)
                        mylist.Add(dico);
                }
                ViewBag.ListPub=mylist;
                return View();
                    
            }
           
        }

        // GET: PublicationController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PublicationController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PublicationController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PublicationController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PublicationController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PublicationController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PublicationController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
