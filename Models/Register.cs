using System.ComponentModel.DataAnnotations;

namespace NetAtlas.Models
{
    public class Register
    {
        [Key]
        public int Id { get; set; }

        [StringLength(50)]
        public string Nom { get; set; }

        [StringLength(100)]
        public string Prenom { get; set; }

        [Required, DataType(DataType.EmailAddress),StringLength(30,ErrorMessage ="Cet adresse mail n'est pas valide")]
        public string Email { get; set; }

        [Display(Name = "Mot de passe utilisateur")]
        [MinLength(8, ErrorMessage = "Mot de passe trop court"), DataType(DataType.Password)]
        [Required]
        public string Password { get; set; }

        [Display(Name = " Confirmer mot de passe utilisateur")]
        [Compare("Password",ErrorMessage ="Mot de passe invalide"), DataType(DataType.Password)]
        [Required]
        public string ConfirmPassword { get; set; }
    }
}
