using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DependencySummary.Models
{
    public class Package
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Version { get; set; }

        public string TargetFramework { get; set; }

        public List<Project> Projects { get; set; }

        public int ComponentId { get; set; }

        public virtual Component Component { get; set; }
    }
}