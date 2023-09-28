using Microsoft.EntityFrameworkCore;
using Rental.Models;

namespace Rental.Data
{
    public class RentalContext : DbContext
    {
        public RentalContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<PersonModel> Persons { get; set; }
        public DbSet<ItemModel> Items { get; set; }
        public DbSet<OrderModel> Orders { get; set; }
    }
}
