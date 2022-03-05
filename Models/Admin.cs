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

        public string Nom { get; set; }


        public string Prenom { get; set; }

        [Required]
        public string Email { get; set; }
        public int isLogged { get; set; } = 0;
    }
}
