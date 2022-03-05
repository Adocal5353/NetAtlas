using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace NetAtlas.Models
{
    public class Membre
    {
        [Key]
        public int Id { get; set; }

        [StringLength(50)]
        public string Nom { get; set; }

        [StringLength(100)]
        public string Prenom { get; set; }

        [Required,DataType(DataType.EmailAddress)]
        [Display(Name = "Adresse mail")]
        [StringLength(100)]
        public string Email { get; set; }

        [Display(Name = "Mot de passe utilisateur")]
        [MinLength(8, ErrorMessage = "Le mot de passe est trop court "),DataType(DataType.Password)]
        [Required]
        public string Password { get; set; }

        public int NbrAvertissement { get; set; } = 0;

        [ScaffoldColumn(false)]
        public string? PhotoProfil { get; set; }
        public int isLogged { get; set; } = 0;


        public Membre()
        {

        }
    }
}
