using System.ComponentModel.DataAnnotations;

namespace BokHemsida.Models
{
    public class LogInViewModel
    {
        [Required(ErrorMessage = "Vänligen skriv ett användarnamn.")]
        [StringLength(300)]
        public string Username { get; set; }
        [Required(ErrorMessage = "Vänligen skriv ett lösenord.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
