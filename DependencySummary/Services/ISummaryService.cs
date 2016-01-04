using System.Collections.Generic;

namespace DependencySummary.Services
{
    public interface ISummaryService
    {
        IEnumerable<ViewModels.SummaryPackage> GetSummary();
        ViewModels.Component GetComponentDetails(int componentId);
    }
}