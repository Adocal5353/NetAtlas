#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NetAtlas.Models;

namespace NetAtlas.Data
{
    public class NetAtlasContext : DbContext
    {
        public NetAtlasContext (DbContextOptions<NetAtlasContext> options)
            : base(options)
        {
        }

        public DbSet<NetAtlas.Models.Membre> Membre { get; set; }

        public DbSet<NetAtlas.Models.Register> Register { get; set; }

        public DbSet<NetAtlas.Models.Admin> Admin { get; set; }

        public DbSet<NetAtlas.Models.Moderateur> Moderateur { get; set; }

        public DbSet<NetAtlas.Models.Ressource> Ressource{ get; set; }

        public DbSet<NetAtlas.Models.Publication> Publication{ get; set; }

        public DbSet<NetAtlas.Models.Message> Message { get; set; }

        public DbSet<NetAtlas.Models.PhotoVideo> PhotoVideo { get; set; }

        public DbSet<NetAtlas.Models.Lien> Lien { get; set; }

        public DbSet<NetAtlas.Models.Amitie> Amitie { get; set; }
    }
}
