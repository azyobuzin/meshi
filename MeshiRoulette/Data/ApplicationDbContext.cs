﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MeshiRoulette.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<PlaceCollection> PlaceCollections { get; set; }

        public DbSet<PlaceCollectionParticipant> PlaceCollectionParticipants { get; set; }

        public DbSet<UnregisteredPlaceCollectionParticipant> UnregisteredPlaceCollectionParticipants { get; set; }

        public DbSet<Place> Places { get; set; }

        public DbSet<PlaceTag> PlaceTags { get; set; }

        public DbSet<PlaceTagAssociation> PlaceTagAssociations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PlaceTag>()
                .HasIndex(x => x.Name)
                .IsUnique();

            // Place, PlaceTag の many-to-many リレーション
            modelBuilder.Entity<PlaceTagAssociation>(b =>
            {
                b.HasKey(a => new { a.PlaceId, a.TagId });

                b.HasOne(a => a.Place)
                    .WithMany(p => p.TagAssociations)
                    .HasForeignKey(a => a.PlaceId);

                b.HasOne(a => a.Tag)
                    .WithMany(t => t.Associations)
                    .HasForeignKey(a => a.TagId);
            });

            modelBuilder.Entity<PlaceCollectionParticipant>()
                .HasIndex(x => new { x.PlaceCollectionId, x.UserId })
                .IsUnique();

            modelBuilder.Entity<UnregisteredPlaceCollectionParticipant>()
                .HasIndex(x => x.ExternalId);
        }
    }
}
