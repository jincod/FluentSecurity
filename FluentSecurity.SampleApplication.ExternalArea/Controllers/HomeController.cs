using System.Web.Mvc;

namespace FluentSecurity.SampleApplication.ExternalArea.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			return View();
		}
	}
}