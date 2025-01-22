using Entity.Entity.Models;
using Microsoft.EntityFrameworkCore;

namespace Entity.Entity
{
    public class LvrDbContext : DbContext
    {
        public LvrDbContext(DbContextOptions<LvrDbContext> options) : base(options) { }

        public DbSet<LVR> LVR { get; set; }
    }
}
