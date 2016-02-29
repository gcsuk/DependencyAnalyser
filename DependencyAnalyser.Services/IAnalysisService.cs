using System.Collections.Generic;
using System.Threading.Tasks;

namespace DependencyAnalyser.Services
{
    public interface IAnalysisService
    {
        IEnumerable<Models.Package> Analyse(string targetDirectory);
        Task<bool> Upload(Models.Component component);
    }
}