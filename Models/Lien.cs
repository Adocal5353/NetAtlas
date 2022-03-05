using System.ComponentModel.DataAnnotations;

namespace NetAtlas.Models
{
    public class Lien:Ressource
    {
        [DataType(DataType.Url)]
        public string Url { get; set; }
    }
}
