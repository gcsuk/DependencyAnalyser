using System.ComponentModel.DataAnnotations.Schema;

namespace DependencySummary.Models
{
    public class Project
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }

        public int PackageId { get; set; }

        public virtual Package Package { get; set; }
    }
}