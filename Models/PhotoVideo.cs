using System.ComponentModel.DataAnnotations;

namespace NetAtlas.Models
{
    public class PhotoVideo:Ressource
    {
        public float TailleEnMo { get; set; }
        public String Chemin { get; set; }
        public int TypeMedia { get; set; }  //1 pour photo 2 pour vidéo
    }
}
