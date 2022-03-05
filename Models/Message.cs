using System.ComponentModel.DataAnnotations;

namespace NetAtlas.Models
{
    public class Message:Ressource
    {
        [DataType(DataType.MultilineText)]
        public string contenu { get; set; }
    }
}
