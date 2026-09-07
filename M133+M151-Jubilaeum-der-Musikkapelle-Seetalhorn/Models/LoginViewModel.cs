using System.ComponentModel.DataAnnotations;

namespace M133_M151_Jubilaeum_der_Musikkapelle_Seetalhorn.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Bitte Passwort eingeben")]
        [DataType(DataType.Password)]
        [Display(Name = "Passwort")]
        public string Password { get; set; } = string.Empty;
    }
}
