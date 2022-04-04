using M133_M151_Jubilaeum_der_Musikkapelle_Seetalhorn.Models;
using Microsoft.EntityFrameworkCore;

namespace M133_M151_Jubilaeum_der_Musikkapelle_Seetalhorn.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Seetalhorn> Seetalhorn { get; set; }
    }
}
