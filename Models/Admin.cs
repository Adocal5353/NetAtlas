using System.ComponentModel.DataAnnotations;

namespace NetAtlas.Models
{
    public class Admin
    {

        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Mot de passe de l'administrateur"),DataType(DataType.Password)]
        public string Password { get; set; }

        [StringLength(50)]
        public string Nom { get; set; }

        [StringLength(100)]
        public string Prenom { get; set; }

        [Required, DataType(DataType.EmailAddress)]
        [Display(Name = "Adresse mail")]
        [StringLength(100)]
        public string Email { get; set; }
        public int isLogged { get; set; } = 0;
    }
}
