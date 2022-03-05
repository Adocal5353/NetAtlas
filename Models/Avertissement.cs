using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetAtlas.Models
{
    public class Avertissement
    {
        [Key]
        public int Id { get; set; }
        public String Motif { get; set; }
        public virtual Membre Membre { get; set; }
        [ForeignKey("Membre")]
        public int IdMembre { get; set; }
        [DataType(DataType.MultilineText)]
        public String Message { get; set; }
    }
}
