using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetAtlas.Models
{
    public class Publication
    {
        [Key]
        public int Id { get; set; }
        
        public DateTime DatePublication { get; set; }=DateTime.Now;
        public virtual Membre Menber { get; set; }

        [ForeignKey("Menber")]
        public int IdMemdre { get; set; }
    }
}
