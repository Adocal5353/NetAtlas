using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetAtlas.Data;
using NetAtlas.Models;
using System.Web;
using System.IO;
using System.Net.Mail;

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
            var type = HttpContext.Session.GetString("UserType");
            if (type is not "admin")
            {
                return RedirectToAction("Login", "Membre");
            }
            else
            {
                return View(await BaseDeDonnee.Register.ToListAsync());
            }
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
                BaseDeDonnee.Membre.Add(menber);
                BaseDeDonnee.Remove(register);
                await BaseDeDonnee.SaveChangesAsync();

                try
                {
                    string body = "<h1>Demande d'inscription à Netatlas</h1> <p style='color:green;'> Votre demande d'inscription à NetAtlas a été approuvé.<br/>Connectez-vous";
                    EnvoiMail(menber.Email,body, "Demande d'inscription approuvée");
                    ViewBag.checkEnvoi = true;
                }
                catch (Exception ex)
                {
                    ViewBag.checkEnvoi = false;
                }
            }


            return View(menber);
        }


        // GET:Rejeter
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

  
        public async Task<IActionResult> Refuser(int? id)
        {
            var register = await BaseDeDonnee.Register.FindAsync(id);
            if (register == null)
            {
                return NotFound();
            }
            try
            {
                string body = "<h1>Demande d'inscription à Netatlas</h1> <p style='color:red;'> Votre demande d'inscription à NetAtlas a été rejeter.<br/>C'est la vie";
                EnvoiMail(register.Email, body, "Demande d'inscription rejetée") ;
                ViewBag.checkEnvoi = true;
            }
            catch (Exception)
            {
                ViewBag.checkEnvoi = false;
            }
           
           BaseDeDonnee.Remove(register);
           await BaseDeDonnee.SaveChangesAsync();
           return View(register);
        }



        public void EnvoiMail(string adr,string body,string subject)
        {
            

             MailMessage mail = new MailMessage();
            
            mail.Priority = MailPriority.High;
            mail.IsBodyHtml = true;
            mail.Body = body;
            mail.Subject = subject; 
            mail.From = new MailAddress("netatlas2022@gmail.com");
            mail.To.Add(new MailAddress(adr));

            SmtpClient client = new SmtpClient("smtp.gmail.com",587);
            client.EnableSsl = true;
            client.Credentials = new System.Net.NetworkCredential("netatlas2022@gmail.com", "Qsdf#2022");
            client.Send(mail);

        }


         public async Task<IActionResult> MembresAverti()
        {
            return View(await BaseDeDonnee.Membre.Where(m=>m.NbrAvertissement>=3).ToListAsync());
        }
        public async Task<IActionResult> SuppMembre(int id)
        {
            var menber = await BaseDeDonnee.Membre.FindAsync(id);
            if (menber is null)
            {
                return NotFound();
            }
            return View(menber);
        }

        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ValiderSup(int id)
        {
            var menber = await BaseDeDonnee.Membre.FindAsync(id);
            try
            {
                string body = "<h1>Suppression du compte</h1> <p style='color:red;'>En raison de vos violations multiples des politiques de NetAtlas vous avez été banni<br/>";
                EnvoiMail(menber.Email, body, "Bannissement de NetAtlas");
                ViewBag.checkEnvoi = true;
            }
            catch (Exception ex)
            {
                ViewBag.checkEnvoi = false;
            }
            BaseDeDonnee.Remove(menber);
            await BaseDeDonnee.SaveChangesAsync();
            return View();
        }


        public async Task<IActionResult> RestaureMembre(int id)
        {
            var menber = await BaseDeDonnee.Membre.FindAsync(id);
            if (menber is null)
            {
                return NotFound();
            }
            return View(menber);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ValiderRes(int id)
        {
            var menber = await BaseDeDonnee.Membre.FindAsync(id);
            try
            {
                string body = "<h1>Restauration du compte</h1> <p style='color:green;'>Les compteurs sont remis à zéro<br/>";
                EnvoiMail(menber.Email, body, "Bannissement de NetAtlas");
                ViewBag.checkEnvoi = true;
            }
            catch (Exception)
            {
                ViewBag.checkEnvoi = false;
            }
            menber.NbrAvertissement = 0;
            BaseDeDonnee.Update(menber);
            await BaseDeDonnee.SaveChangesAsync();
            return View(menber);
        }
    }

    
}
