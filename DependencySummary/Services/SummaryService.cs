using System.Collections.Generic;
using System.Linq;
using DependencySummary.Models;

namespace DependencySummary.Services
{
    public class SummaryService : ISummaryService
    {
        public IEnumerable<ViewModels.SummaryPackage> GetSummary()
        {
            List<ViewModels.SummaryPackage> packages;

            using (var db = new PackageContext())
            {
                var packageData = db.Packages.Include("Projects").ToList();
                var componentData = db.Components.ToList();

                packages =
                    packageData.GroupBy(p => p.Name)
                        .Select(f => f.FirstOrDefault())
                        .Select(vm => new ViewModels.SummaryPackage
                        {
                            Id = vm.Id,
                            Name = vm.Name
                        })
                        .OrderBy(o =>  o.Name)
                        .ToList();

                packages.ForEach(package =>
                {
                    package.Versions.AddRange(
                        packageData.Where(v => v.Name == package.Name)
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
                            packageData.Where(
                                p =>
                                    p.Name == package.Name && p.Version == v.Version &&
                                    p.TargetFramework == v.TargetFramework)
                                .Select(
                                    c =>
                                        new ViewModels.SummaryComponent
                                        {
                                            Id = c.ComponentId,
                                            Name = componentData.FirstOrDefault(component => c.ComponentId == component.Id)?.Name,
                                            Projects = c.Projects.Select(pr => new ViewModels.SummaryProject
                                            {
                                                Name = pr.Name
                                            }).ToList()
                                        })
                                .ToList()
                            );
                    });
                });
            }

            return packages;
        }

        public IEnumerable<ViewModels.Component> GetComponentDetails(int componentId, string packageName, string version, string targetFramework)
        {
            List<ViewModels.Component> packages;

            using (var db = new PackageContext())
            {
                packages = db.Components
                    .Where(component => component.Id == componentId)
                    .Select(c => new ViewModels.Component
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Packages =
                            c.Packages.Where(
                                package =>
                                    package.Name == packageName && package.Version == version &&
                                    package.TargetFramework == targetFramework).Select(p => new ViewModels.Package
                                    {
                                        Name = c.Name,
                                        Version = p.Version,
                                        TargetFramework = p.TargetFramework,
                                        Projects = p.Projects.Select(pr => new ViewModels.Project
                                        {
                                            Name = pr.Name
                                        }).ToList()
                                    }).ToList()
                    }).ToList();
            }

            return packages;
        }
    }
}