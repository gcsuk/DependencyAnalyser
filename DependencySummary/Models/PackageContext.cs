using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace DependencySummary.Models
{
    public class PackageContext : DbContext
    {
        public PackageContext()
            : base("name=PackageContext")
        {
        }

        public virtual DbSet<Component> Components { get; set; }

        public virtual DbSet<Package> Packages { get; set; }

        public virtual DbSet<Project> Projects { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Component>()
                .Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<Package>()
                .Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<Project>()
                .Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<Package>()
                .HasRequired(c => c.Component)
                .WithMany(d => d.Packages)
                .HasForeignKey(d => new {d.ComponentId});

            modelBuilder.Entity<Package>()
                .HasRequired(t => t.Component)
                .WithMany(t => t.Packages)
                .HasForeignKey(d => d.ComponentId)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Project>()
                .HasRequired(c => c.Package)
                .WithMany(d => d.Projects)
                .HasForeignKey(d => new { d.PackageId });

            modelBuilder.Entity<Project>()
                .HasRequired(t => t.Package)
                .WithMany(t => t.Projects)
                .HasForeignKey(d => d.PackageId)
                .WillCascadeOnDelete(true);

            base.OnModelCreating(modelBuilder);
        }
    }
}