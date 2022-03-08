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
                BaseDeDonnee.Membre.Add(menber);
                BaseDeDonnee.Remove(register);
                await BaseDeDonnee.SaveChangesAsync();

                //EnvoiMessValidation(menber.Email);
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
           //EnvoiMessRefuser(register.Email);
           BaseDeDonnee.Remove(register);
           await BaseDeDonnee.SaveChangesAsync();
           return View(register);
        }

       



        private void EnvoiMessValidation(string adr)
        {
            string body="<h1>Demande d'inscription à Netatlas</h1> <p style='color:green;'> Votre demande d'inscription à NetAtlas a été approuvé.<br/>Connectez-vous";

            MailMessage mail= new MailMessage("", adr,"Demande acceptée",body);
            SmtpClient client= new SmtpClient("smtp.gmail.com");
            client.Port = 587;
            client.EnableSsl = true;
            client.Credentials=new System.Net.NetworkCredential("", "");
            client.Send(mail);

        }


        private void EnvoiMessRefuser(string adr)
        {
            string body = "<h1>Demande d'inscription à Netatlas</h1> <p style='color:red;'> Votre demande d'inscription à NetAtlas a été rejeter.<br/>C'est la vie";

            MailMessage mail = new MailMessage();
            
            mail.Priority = MailPriority.High;
            mail.IsBodyHtml = true;
            mail.Body = body;
            mail.Subject = "Demande d'inscription rejetée";
            mail.From = new MailAddress("markonolitse@gmail.com");
            mail.To.Add(new MailAddress(adr));

            SmtpClient client = new SmtpClient("smtp.gmail.com",587);
            client.EnableSsl = true;
            client.Credentials = new System.Net.NetworkCredential("markonolitse@gmail.com", "");
            client.Send(mail);

        }


         public async Task<IActionResult> MembresAverti()
        {
            return View(await BaseDeDonnee.Membre.Where(m=>m.NbrAvertissement>=3).ToListAsync());
        }

        
    }

    
}
