using Microsoft.AspNetCore.Mvc;
using NetAtlas.Data;
using NetAtlas.Models;
using System.Net.Mime;
using System.Text.Json;
using System.Web.Helpers;
using System.Web;
using System.IO;

namespace NetAtlas.Controllers
{
    public class PhotoVideosController : Controller
    {
        private readonly NetAtlasContext _context;

        public PhotoVideosController(NetAtlasContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public Membre? GetMembre()
        {
            var value = HttpContext.Session.GetString("UserSession");
            return value == null ? null : JsonSerializer.Deserialize<Membre>(value);
        }

        public ActionResult Create()
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
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormFile file, string nomRessource)
        {
            var type = HttpContext.Session.GetString("UserType");
            if (type is not "membre")
            {
                return Unauthorized();
            }
            List<String> ListVideo = new List<String>()
            {
                "video/mpeg",
                "video/mp2t",
                "video/webm",
                "video/3gpp",
                "video/3gp"
                
            };
            List<String> ListPhoto = new List<String>()
            {
                "image/jpeg",
                "image/png",
                "image/tiff",
                "image/avif",
                "image/gif",
                "image/bmp",
                "image/svg+xml",
                "image/webp"
            };

            if(file != null && file.Length>0)
            {
                if (ListPhoto.Contains(file.ContentType))
                {
                    var user = GetMembre();
                    Publication p = new Publication();
                    p.DatePublication = DateTime.Now;
                    p.IdMemdre = user.Id;
                    _context.Publication.Add(p);
                    await _context.SaveChangesAsync();

                    var fileName = Guid.NewGuid().ToString()+'_' + Path.GetFileName(file.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot/images", fileName);

                    PhotoVideo photo = new PhotoVideo()
                    {
                        IdPublication = p.Id,
                        nomRessource = nomRessource,
                        Chemin = filePath,
                        TypeMedia = 1,
                        TailleEnMo = file.Length
                    };
                    _context.PhotoVideo.Add(photo);
                    await _context.SaveChangesAsync();
                    using(var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                    return RedirectToAction(nameof(Index));
                }
                else if (ListVideo.Contains(file.ContentType))
                {
                    var user = GetMembre();
                    ViewBag.Membre = user.Nom + " " + user.Prenom;
                    Publication p = new Publication();
                    p.DatePublication = DateTime.Now;
                    p.IdMemdre = user.Id;
                    _context.Publication.Add(p);
                    await _context.SaveChangesAsync();

                    var fileName = Guid.NewGuid().ToString() + '_' + Path.GetFileName(file.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/videos", fileName);

                    PhotoVideo photo = new PhotoVideo()
                    {
                        IdPublication = p.Id,
                        nomRessource = nomRessource,
                        Chemin = filePath,
                        TypeMedia = 2,
                        TailleEnMo = file.Length
                    };
                    _context.PhotoVideo.Add(photo);
                    await _context.SaveChangesAsync();
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                    return RedirectToAction(nameof(Index));

                }
                else
                {
                    ViewBag.Message = "Le format de votre fichier n'est pas valide. veuillez en choisir un autre";
                    return View();
                }
            }

            ViewBag.Message="Votre fichier est invalide ou endomagé";
            return View();

        }
    }
}
