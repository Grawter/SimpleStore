using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SimpleStore.Models.Booking;
using SimpleStore.Models.Shop;


namespace SimpleStore.Models
{
    public class ApplicationContext : IdentityDbContext<User>
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }
    }
}