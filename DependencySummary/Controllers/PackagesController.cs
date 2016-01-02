using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DependencySummary.Controllers
{
    public class PackagesController : ApiController
    {
        // POST api/<controller>
        public HttpResponseMessage Post([FromBody]ViewModels.Component component)
        {
            try
            {
                using (var db = new Models.PackageContext())
                {
                    if (!db.Components.Any(c => c.Id == component.Id))
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound);
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

                    return Request.CreateResponse(HttpStatusCode.OK, component);
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }
    }
}