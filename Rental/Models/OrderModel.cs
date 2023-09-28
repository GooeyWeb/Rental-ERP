using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rental.Models
{
    [Table("Orders")]
    public class OrderModel
    {
        [Key]
        public int OrderID { get; set; }

        [MaxLength(11)]
        public string? PersonPESEL { get; set; }

        [DisplayName("Data zawarcia umowy")]
        public DateTime LoanDate { get; set; } = DateTime.Now;

        [DisplayName("Data wykonania umowy")]
        public DateTime? ReturnDate { get; set; }
        
        //Wypożyczenie
        public int? ItemID { get; set; }
        //Naprawa
        public string? ItemNameForRepair { get; set; }

        public int? Amount { get; set; }

		[Precision(18, 2)]
		public decimal? CostPerHour { get; set; }

		[Precision(18, 2)]
		public decimal? CostPerDay { get; set; }

        [DisplayName("Koszt")]
		[Precision(18, 2)]
		public decimal? Cost { get; set; }

        [DisplayName("Zaliczka")]
        [Precision(18, 2)]
        public decimal? Advance { get; set; }

        [DisplayName("Zapłacono")]
        public bool Paid { get; set; }
    }
}