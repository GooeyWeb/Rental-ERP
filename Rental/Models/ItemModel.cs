using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rental.Models
{
    [Table("Items")]
    public class ItemModel
    {
        [Key]
        public int ItemID { get; set; }

        [DisplayName("Nazwa")]
        [Required(ErrorMessage = "Podanie nazwy przedmiotu jest obowiązkowe")]
        [MaxLength(100)]
        public string? Name { get; set; }

        [DisplayName("Koszt na dzień")]
		[Precision(18, 2)]
		public decimal? CostPerDay { get; set; }

        [DisplayName("Koszt na godzinę")]
		[Precision(18, 2)]
		public decimal? CostPerHour { get; set; }
        
        [DisplayName("Dostępna ilość")]
        [Required(ErrorMessage = "Podanie ilości dostępnego przedmiotu jest obowiązkowe")]
        public int Availability { get; set; }
    }
}
