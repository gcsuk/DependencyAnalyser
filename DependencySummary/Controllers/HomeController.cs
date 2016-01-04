using System.Web.Mvc;

namespace DependencySummary.Controllers
{
    public class HomeController : Controller
    {
        private readonly Services.ISummaryService _summaryService;

        public HomeController(Services.ISummaryService summaryService)
        {
            _summaryService = summaryService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ComponentDetails(int componentId)
        {
            return View("ComponentDetails", componentId);
        }
    }
}
