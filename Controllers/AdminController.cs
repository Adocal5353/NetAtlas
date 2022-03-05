using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetAtlas.Data;
using NetAtlas.Models;

namespace NetAtlas.Controllers
{
    public class AdminController : Controller
    {
        private readonly NetAtlasContext BaseDeDonnee;
        public AdminController(NetAtlasContext context)
        {
            BaseDeDonnee = context;
        }
        
        
        //Voir la liste des demandes d'inscriptions non traitées
        public async Task<IActionResult> Index()
        {
            return View( await BaseDeDonnee.Register.ToListAsync());
        }

        // GET:Autoristion
        //Voir les en détails les informations du register sur un formulaire 
        public async Task<IActionResult> Autorisation(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var register = await BaseDeDonnee.Register.FindAsync(id);
            if (register == null)
            {
                return NotFound();
            }
            return View(register);
        }


        public async Task<IActionResult> Valider(int? id)
        {
            Membre menber = new Membre();
            var register = await BaseDeDonnee.Register.FindAsync(id);
            if (register == null)
            {
                return NotFound();
            }
            else
            {
                menber.Nom = register.Nom;
                menber.Prenom = register.Prenom;
                menber.Email = register.Email;
                menber.Password = register.Password;
                //BaseDeDonnee.Add(menber);
                BaseDeDonnee.Membre.Add(menber);
                BaseDeDonnee.Remove(register);
                await BaseDeDonnee.SaveChangesAsync();
            }


            return View(menber);
        }


        // GET:Autoristion
        //Voir les en détails les informations du register sur un formulaire 
        public async Task<IActionResult> Rejeter(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var register = await BaseDeDonnee.Register.FindAsync(id);
            if (register == null)
            {
                return NotFound();
            }
            return View(register);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Rejeter(Register register)
        {
          
           BaseDeDonnee.Remove(register);
           await BaseDeDonnee.SaveChangesAsync();
           return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> MembresAverti()
        {
            return View(await BaseDeDonnee.Membre.Where(m=>m.NbrAvertissement>=3).ToListAsync());
        }

        
    }
    
}
