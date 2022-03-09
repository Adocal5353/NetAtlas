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
                var q1 =
                    from c1 in _context.Amitie
                    from p1 in _context.Publication
                    where  c1.Statut == 1 && (c1.IdSender == user.Id || c1.IdReceiver==user.Id)
                    select p1;
                foreach(var item in q1)
                {
                    var dico = new Dictionary<string, object>();
                    dico["publication"] = item;
                    var res = _context.Lien.FirstAsync(r => r.IdPublication == item.Id);
                    var res2= _context.Message.FirstAsync(r => r.IdPublication == item.Id);
                    var res3= _context.PhotoVideo.FirstAsync(r => r.IdPublication == item.Id);
                    if(res is not null)
                    {
                        dico["ressource"]=res;
                    } else if(res2 is not null)
                    {
                        dico["ressource"] = res2;
                    }else if(res3 is not null)
                    {
                        dico["ressource"] = res3;
                    }

                    
                    ViewBag.ListPub.Add(dico);
                }
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
