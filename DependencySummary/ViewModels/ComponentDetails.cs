using System.Collections.Generic;

namespace DependencySummary.ViewModels
{
    public class ComponentDetails
    {
        public int ComponentId { get; set; }

        public IEnumerable<Models.Component> Components { get; set; }
    }
}