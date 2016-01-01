using System;
using System.Collections.Generic;

namespace DependencyAnalyser.Models
{
    public class Package : IComparable<Package>
    {
        public Package()
        {
            Projects = new List<Project>();
        }

        public string Name { get; set; }

        public string Version { get; set; }

        public string TargetFramework { get; set; }

        public List<Project> Projects { get; set; }

        public override string ToString()
        {
            return Name + Version + TargetFramework;
        }

        public int CompareTo(Package other)
        {
            return other.Name == Name && other.Version == Version && other.TargetFramework == TargetFramework ? 1 : 0;
        }
    }
}
