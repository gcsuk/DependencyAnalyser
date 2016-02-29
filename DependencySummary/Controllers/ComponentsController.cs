using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DependencySummary.Services;

namespace DependencySummary.Controllers
{
    public class ComponentsController : ApiController
    {
        private readonly IComponentService _componentService;

        public ComponentsController(IComponentService componentService)
        {
            _componentService = componentService;
        }

        // GET api/<controller>
        public HttpResponseMessage Get()
        {
            try
            {
                var components = _componentService.GetList().Select(c => new ViewModels.Component
                {
                    Id = c.Id,
                    Name = c.Name
                });

                return Request.CreateResponse(HttpStatusCode.OK, components.ToList());
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        // GET api/<controller>/5
        public HttpResponseMessage Get(int id)
        {
            try
            {
                var component = _componentService.GetItem(id);

                if (component == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }

                return Request.CreateResponse(HttpStatusCode.OK, new ViewModels.Component
                {
                    Id = component.Id,
                    Name = component.Name,
                    TeamCityBuildId = component.TeamCityBuildId
                });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        // GET api/<controller>/bt5
        public HttpResponseMessage Get(string teamCityBuildId)
        {
            try
            {
                var component = _componentService.GetItem(teamCityBuildId);

                if (component == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }

                return Request.CreateResponse(HttpStatusCode.OK, new ViewModels.Component
                {
                    Id = component.Id,
                    Name = component.Name,
                    TeamCityBuildId = component.TeamCityBuildId
                });
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
                var matchFound = _componentService.Update(component);

                return !matchFound
                    ? Request.CreateResponse(HttpStatusCode.NotFound)
                    : Request.CreateResponse(HttpStatusCode.OK, component);
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
                component.Id = _componentService.Add(component);

                return Request.CreateResponse(HttpStatusCode.OK, component);
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
                _componentService.Delete(id);

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }
    }
}