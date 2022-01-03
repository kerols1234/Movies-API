using Microsoft.EntityFrameworkCore;
using Movies_API.Models;

namespace AngularAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Movies> Movies { get; set; }
        public DbSet<Review> Reviews { get; set; }
    }
}