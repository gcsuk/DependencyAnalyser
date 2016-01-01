using System.ComponentModel.DataAnnotations.Schema;

namespace DependencySummary.Models
{
    public class Project
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }
    }
}