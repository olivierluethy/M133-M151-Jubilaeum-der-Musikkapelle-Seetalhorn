using System.ComponentModel.DataAnnotations;

namespace M133_M151_Jubilaeum_der_Musikkapelle_Seetalhorn.Models
{
    public class Seetalhorn
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "E-Mail")]
        [Required(ErrorMessage = "{0} ist Pflichtfeld")]
        [EmailAddress(ErrorMessage = "{0} muss eine valide E-Mail Adresse sein")]
        [MaxLength(200, ErrorMessage = "{0} darf max. 200 Zeichen lang sein")]
        public string Email { get; set; } = string.Empty;
        [Display(Name = "Antwort1")]
        [Required(ErrorMessage = "Frage 1 ist Pflichtfeld")]
        public string Antwort1 { get; set; } = string.Empty;
        [Display(Name = "Antwort2")]
        [Required(ErrorMessage = "Frage 2 ist Pflichtfeld")]
        public string Antwort2 { get; set; } = string.Empty;
        [Display(Name = "Antwort3")]
        [Required(ErrorMessage = "Frage 3 ist Pflichtfeld")]
        public string Antwort3 { get; set; } = string.Empty;
        [Display(Name = "Antwort4")]
        [Required(ErrorMessage = "Frage 4 ist Pflichtfeld")]
        public string Antwort4 { get; set; } = string.Empty;
        [Display(Name = "Antwort5")]
        [Required(ErrorMessage = "Frage 5 ist Pflichtfeld")]
        public string Antwort5 { get; set; } = string.Empty;
        [Display(Name = "Punkte")]
        public int Punkte { get; set; }
        [Display(Name = "Gewinner")]
        public bool IsWinner { get; set; }
    }
}
