using System.ComponentModel.DataAnnotations.Schema;

namespace NetAtlas.Models
{
    public class Ressource
    {
        public int Id { get; set; }

        public virtual Publication Publication { get; set; }

        [ForeignKey("Publication")]
        public int IdPublication { get; set; }
        public string nomRessource { get; set; }

        //if it is a message type=0, a link type = 1, an image type=2 and a video type=3
        public int Type { get; set; }
    }
}
