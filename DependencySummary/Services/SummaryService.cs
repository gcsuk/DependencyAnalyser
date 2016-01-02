using System.Collections.Generic;
using System.Linq;
using DependencySummary.Models;

namespace DependencySummary.Services
{
    public class SummaryService
    {
        public IEnumerable<ViewModels.SummaryPackage> GetSummary()
        {
            List<ViewModels.SummaryPackage> packages;

            using (var db = new PackageContext())
            {
                packages =
                    db.Packages.GroupBy(p => p.Name)
                        .Select(f => f.FirstOrDefault())
                        .Select(vm => new ViewModels.SummaryPackage
                        {
                            Name = vm.Name
                        }).ToList();

                packages.ForEach(package =>
                {
                    package.Versions.AddRange(
                        db.Packages.Where(v => v.Name == package.Name)
                            .GroupBy(g => new {g.Version, g.TargetFramework})
                            .Select(f => f.FirstOrDefault())
                            .Select(version => new ViewModels.SummaryVersion
                            {
                                Version = version.Version,
                                TargetFramework = version.TargetFramework
                            }));

                    package.Versions.ForEach(v =>
                    {
                        v.Components.AddRange(
                            db.Packages.Where(
                                p =>
                                    p.Name == package.Name && p.Version == v.Version &&
                                    p.TargetFramework == v.TargetFramework)
                                .Select(
                                    c =>
                                        new ViewModels.SummaryComponent
                                        {
                                            Id = c.ComponentId,
                                            Name = db.Components.FirstOrDefault(component => c.ComponentId == component.Id).Name
                                        })
                                .ToList()
                            );
                    });
                });
            }

            return packages;
        }
    }
}