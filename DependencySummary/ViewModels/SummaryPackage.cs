using System.Collections.Generic;

namespace DependencySummary.ViewModels
{
    public class SummaryPackage
    {
        public SummaryPackage()
        {
            Versions = new List<SummaryVersion>();
        }

        public string Name { get; set; }

        public List<SummaryVersion> Versions { get; set; }
    }
}