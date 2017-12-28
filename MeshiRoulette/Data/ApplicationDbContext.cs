using Microsoft.EntityFrameworkCore;

namespace MeshiRoulette.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<ApplicationUser> Users { get; set; }

        public DbSet<PlaceCollection> PlaceCollections { get; set; }

        public DbSet<Place> Places { get; set; }

        public DbSet<PlaceTag> PlaceTags { get; set; }

        public DbSet<PlaceTagAssociation> PlaceTagAssociations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // PlaceTag の複合 Unique
            modelBuilder.Entity<PlaceTag>()
                .HasIndex(x => new { x.Name, x.PlaceCollectionId })
                .IsUnique();

            // Place, PlaceTag の many-to-many リレーション
            modelBuilder.Entity<PlaceTagAssociation>(e =>
            {
                e.HasKey(a => new { a.PlaceId, a.TagId });

                e.HasOne(a => a.Place)
                    .WithMany(p => p.TagAssociations)
                    .HasForeignKey(a => a.PlaceId);

                e.HasOne(a => a.Tag)
                    .WithMany(t => t.Associations)
                    .HasForeignKey(a => a.TagId);
            });
        }
    }
}
