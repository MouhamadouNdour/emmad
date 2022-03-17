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
            model.Entity<Photo>()
                .HasOne(p => p.Client)
                .WithMany(c => c.Photos)
                .HasForeignKey(p => p.id_client);

            model.Entity<Organisation>()
                .HasOne(o => o.Administrateur)
                .WithMany()
                .HasForeignKey(o => o.id_administrateur);

            model.Entity<Rdv>()
                .HasOne(r => r.Client)
                .WithMany()
                .HasForeignKey(r => r.id_client);

            model.Entity<Client>()
                .HasOne(o => o.Organisation)
                .WithMany()
                .HasForeignKey(o => o.id_organisation);
        }

        public DbSet<Administrateur> administrateur { get; set; }

        public DbSet<Client> client { get; set; }

        public DbSet<Organisation> organisation { get; set; }

        public DbSet<Rdv> rdv { get; set; }

        public DbSet<Photo> photo { get; set; }

    }
}
