using Microsoft.AspNetCore.Mvc;
using NetAtlas.Data;
using NetAtlas.Models;
using System.Text.Json;
using System.Web.Helpers;

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

        public async Task<IActionResult> Create(WebImage image, string nomRessource)
        {
            var type = HttpContext.Session.GetString("UserType");
            if (type is not "membre")
            {
                return Unauthorized();
            }
            if(image is not null)
            {
                var user = GetMembre();
                Publication p = new Publication();
                p.DatePublication = DateTime.Now;
                p.IdMemdre = user.Id;
                _context.Publication.Add(p);
                await _context.SaveChangesAsync();
                var FileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(image.FileName);
                var imagePath = @"images\" + FileName;
                image.Save(@"~\"+imagePath);
                return RedirectToAction(nameof(Index));
            }
            
            return View();

        }
    }
}
