using System.Web.Mvc;

namespace FluentSecurity.SampleApplication.ExternalArea.Controllers
{
	public class AdminController : Controller
	{
		public ActionResult Index()
		{
			return View();
		}
	}
}