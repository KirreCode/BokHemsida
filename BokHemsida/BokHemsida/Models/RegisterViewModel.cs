using System.ComponentModel.DataAnnotations;

namespace BokHemsida.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Vänligen skriv ett Förnamn.")]
        [StringLength(300)]
        public string Firstname { get; set; }

        [Required(ErrorMessage = "Vänligen skriv ett Efternamn.")]
        [StringLength(300)]
        public string Lastname { get; set; }

        [StringLength(300)]
        [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "Användarnamnet får endast innehålla bokstäver och siffror.")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "Vänligen skriv ett lösenord.")]
        [DataType(DataType.Password)]
        [Compare("ConfirmedPassword", ErrorMessage = "Lösenordet och bekräfta lösenordet matchar inte.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Vänligen upprepa ditt angivna lösenord.")]
        [DataType(DataType.Password)]
        [Display(Name = "Bekräfta Lösenord")]
        public string ConfirmedPassword { get; set; }
    }
}
