using System.Collections.Generic;
using System.Linq;

namespace DependencySummary.Services
{
    public class PackageService
    {
        public bool Update(ViewModels.Component component)
        {
            using (var db = new Models.PackageContext())
            {
                if (!db.Components.Any(c => c.Id == component.Id))
                {
                    return false;
                }

                var existingPackages =
                    db.Components.Include("Packages").Single(c => c.Id == component.Id).Packages;

                if (existingPackages != null)
                {
                    db.Packages.RemoveRange(existingPackages);
                }
                else
                {
                    db.Components.Single(c => c.Id == component.Id).Packages = new List<Models.Package>();
                }

                db.Components.Single(c => c.Id == component.Id)
                    .Packages.AddRange(component.Packages.Select(p => new Models.Package
                    {
                        Name = p.Name,
                        Version = p.Version,
                        TargetFramework = p.TargetFramework,
                        Projects = p.Projects.Select(pr => new Models.Project
                        {
                            Name = pr.Name
                        }).ToList()
                    }));

                db.SaveChanges();

                return true;
            }
        }
    }
}