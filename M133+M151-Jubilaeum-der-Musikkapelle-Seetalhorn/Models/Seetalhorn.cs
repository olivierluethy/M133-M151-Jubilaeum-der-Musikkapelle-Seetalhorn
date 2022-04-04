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
        public string Email { get; set; }
        [Display(Name = "Punkte")]
        [Required(ErrorMessage = "{0} ist Pflichtfeld")]
        public int Punkte { get; set; }
    }
}
