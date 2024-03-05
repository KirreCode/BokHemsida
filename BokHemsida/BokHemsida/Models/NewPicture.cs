using System.ComponentModel.DataAnnotations;

namespace ASP.NET_CVProjekt.Models
{
    public class NewPicture
    {
        [Required(ErrorMessage = "En bild måste väljas.")]
        public IFormFile? Picture { get; set; }
    }
}
