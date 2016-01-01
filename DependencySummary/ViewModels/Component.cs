using System.Collections.Generic;

namespace DependencySummary.ViewModels
{
    public class Component
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<Package> Packages { get; set; }
    }
}