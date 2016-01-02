using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DependencySummary.Services;

namespace DependencySummary.Controllers
{
    public class PackagesController : ApiController
    {
        private readonly PackageService _packageService;

        public PackagesController()
        {
            _packageService = new PackageService();
        }

        // POST api/<controller>
        public HttpResponseMessage Post([FromBody] ViewModels.Component component)
        {
            try
            {
                var matchFound = _packageService.Update(component);

                return matchFound
                    ? Request.CreateResponse(HttpStatusCode.OK, component)
                    : Request.CreateResponse(HttpStatusCode.NotFound);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }
    }
}