using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DependencySummary.Models
{
    public class Component
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }

        public List<Package> Packages { get; set; }
    }
}