using System.Collections.Generic;

namespace DependencyAnalyser.Models
{
    public class Component
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string TeamCityBuildId { get; set; }

        public List<Package> Packages { get; set; }
    }
}