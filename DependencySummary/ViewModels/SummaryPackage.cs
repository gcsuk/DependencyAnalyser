using System.Collections.Generic;

namespace DependencySummary.ViewModels
{
    public class SummaryPackage
    {
        public SummaryPackage()
        {
            Versions = new List<SummaryVersion>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public List<SummaryVersion> Versions { get; set; }

        public int VersionCount => Versions.Count;
    }
}