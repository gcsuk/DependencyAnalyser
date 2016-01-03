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
        private readonly SummaryService _summaryService;

        public SummaryController()
        {
            _summaryService = new SummaryService();
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
    }
}