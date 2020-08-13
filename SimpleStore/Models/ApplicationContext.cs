using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SimpleStore.Controllers;
using SimpleStore.Models.Shop;

namespace SimpleStore.Models
{
    public class ApplicationContext : IdentityDbContext<User>
    {
        public DbSet<Case> Cases { get; set; }
        public DbSet<Headphone> Headphones { get; set; }
        public DbSet<Phone> Phones { get; set; }
        public DbSet<Powerbank> Powerbanks { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {

            Database.EnsureCreated();
        }
    }
}