using System.Collections.Generic;

namespace DependencySummary.Services
{
    public interface ISummaryService
    {
        IEnumerable<ViewModels.SummaryPackage> GetSummary();
        IEnumerable<ViewModels.Component> GetComponentDetails(int componentId, string packageName, string version, string targetFramework);
    }
}