using emmad.Entity;
using Microsoft.EntityFrameworkCore;

namespace emmad.Context
{
    public class MasterContext : DbContext
    {
        public MasterContext(DbContextOptions<MasterContext> options) : base(options) { }
        public MasterContext() { }

        protected override void OnModelCreating(ModelBuilder model)
        {

        }

        public DbSet<Administrateur> administrateur { get; set; }

    }
}
