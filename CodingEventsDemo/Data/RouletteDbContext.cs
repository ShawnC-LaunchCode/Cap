using CodingEventsDemo.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CodingEventsDemo.Data
{
    public class RouletteDbContext : IdentityDbContext<IdentityUser>
    {
        

        public RouletteDbContext(DbContextOptions<RouletteDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            

            base.OnModelCreating(modelBuilder);
        }
    }
}
