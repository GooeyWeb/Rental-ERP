using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rental.Models
{
    [Table("Persons")]
    public class PersonModel
    {
        [Key]
        [MaxLength(11)]
        [Required(ErrorMessage = "Wprowadzenie numeru PESEL jest obowiązkowe")]
        public string PESEL { get; set; }

        [DisplayName("Numer dowodu")]
        [MaxLength(9)]
        public string? IDnumber { get; set; }

        [DisplayName("Imię")]
        [MaxLength(25)]
        [Required(ErrorMessage = "Brak imienia")]
        public string FirstName { get; set; }

        [DisplayName("Nazwisko")]
        [MaxLength(50)]
        [Required(ErrorMessage = "Brak nazwiska")]
        public string LastName { get; set; }

        [DisplayName("Miasto")]
        [MaxLength(50)]
        public string? City { get; set; }

        [DisplayName("Adres")]
        [MaxLength(100)]
        public string? Address { get; set; }

        [DisplayName("Numer telefonu")]
        public int? TelephoneNumber { get; set; }

        [DisplayName("Opis")]
        [MaxLength(2000)]
        public string? Description { get; set; }

    }
}
