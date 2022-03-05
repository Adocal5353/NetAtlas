using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetAtlas.Models
{
    public class Amitie
    {
        [Key]
        public int Id { get; set; }
        public int IdSender { get; set; }

        public virtual Membre Receiver { get; set; }

        [ForeignKey("Receiver")]
        public int IdReceiver { get; set; }
        //Statut is 0 at default. it means they are not yet friend
        public int Statut { get; set; } = 0;
    }
}
