using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using DependencySummary.Services;

namespace DependencySummary.Controllers
{
    public class SummaryController : ApiController
    {
        private readonly ISummaryService _summaryService;

        public SummaryController(ISummaryService summaryService)
        {
            _summaryService = summaryService;
        }

        // GET api/<controller>
        public async Task<HttpResponseMessage> Get()
        {
            try
            {
                var packages = await Task.Run(() => _summaryService.GetSummary());

                return Request.CreateResponse(HttpStatusCode.OK, packages);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        // GET api/<controller>
        public async Task<HttpResponseMessage> Get(int componentId)
        {
            try
            {
                var packages = await Task.Run(() => _summaryService.GetComponentDetails(componentId));

                return Request.CreateResponse(HttpStatusCode.OK, packages);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }
    }
}