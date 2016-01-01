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
    }
}