using System.Collections.Generic;

namespace DependencySummary.ViewModels
{
    public class SummaryVersion
    {
        public SummaryVersion()
        {
            Components = new List<SummaryComponent>();
        }

        public string Version { get; set; }

        public string TargetFramework { get; set; }

        public List<SummaryComponent> Components { get; set; }
    }
}