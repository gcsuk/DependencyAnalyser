using System.Collections.Generic;

namespace DependencySummary.ViewModels
{
    public class SummaryComponent
    {
        public SummaryComponent()
        {
            Projects = new List<SummaryProject>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public List<SummaryProject> Projects { get; set; }
    }
}