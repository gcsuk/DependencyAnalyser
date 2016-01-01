using System.Collections.Generic;

namespace DependencySummary.ViewModels
{
    public class Package
    {
        public string Name { get; set; }

        public string Version { get; set; }

        public string TargetFramework { get; set; }

        public List<Project> Projects { get; set; }
    }
}