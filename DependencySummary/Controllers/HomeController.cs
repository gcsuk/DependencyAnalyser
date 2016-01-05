using System.Web.Mvc;

namespace DependencySummary.Controllers
{
    public class HomeController : Controller
    {
        private readonly Services.ISummaryService _summaryService;
        private readonly Services.IComponentService _componentService;

        public HomeController(Services.ISummaryService summaryService, Services.IComponentService componentService)
        {
            _summaryService = summaryService;
            _componentService = componentService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ComponentDetails(int componentId)
        {
            var vm = new ViewModels.ComponentDetails
            {
                ComponentId = componentId,
                Components = _componentService.GetList()
            };

            return View("ComponentDetails", vm);
        }

        public ActionResult Help()
        {
            return View();
        }
    }
}
