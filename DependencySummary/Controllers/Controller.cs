using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DependencySummary.Controllers
{
    public class ComponentsController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<ViewModels.Component> Get()
        {
            using (var db = new Models.PackageContext())
            {
                var components = db.Components.Select(c => new ViewModels.Component
                {
                    Id = c.Id,
                    Name = c.Name
                });

                return components.ToList();
            }
        }

        // GET api/<controller>/5
        public HttpResponseMessage Get(int id)
        {
            try
            {
                using (var db = new Models.PackageContext())
                {
                    var component = db.Components.SingleOrDefault(c => c.Id == id);

                    if (component == null)
                    {
                        return null;
                    }

                    return Request.CreateResponse(HttpStatusCode.OK, new ViewModels.Component
                    {
                        Id = component.Id,
                        Name = component.Name
                    });

                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        // POST api/<controller>
        public HttpResponseMessage Post([FromBody]ViewModels.Component component)
        {
            try
            {
                using (var db = new Models.PackageContext())
                {
                    var existingComponent = db.Components.SingleOrDefault(c => c.Id == component.Id);

                    if (existingComponent == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound);
                    }

                    existingComponent.Name = component.Name;

                    db.SaveChanges();

                    return Request.CreateResponse(HttpStatusCode.OK, existingComponent);
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        // PUT api/<controller>/5
        public HttpResponseMessage Put([FromBody]ViewModels.Component component)
        {
            try
            {
                var newComponent = new Models.Component {Name = component.Name};

                using (var db = new Models.PackageContext())
                {
                    db.Components.Add(newComponent);

                    db.SaveChanges();
                }

                return Request.CreateResponse(HttpStatusCode.OK, newComponent);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        // DELETE api/<controller>/5
        public HttpResponseMessage Delete(int id)
        {
            try
            {
                using (var db = new Models.PackageContext())
                {
                    db.Components.Remove(db.Components.Single(c => c.Id == id));

                    db.SaveChanges();
                }

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }
    }
}