using System.Web.Mvc;

namespace FluentSecurity.SampleApplication.Controllers
{
	public class CacheController : Controller
	{
		public ActionResult PolicyLevel()
		{
			return View();
		}

		public ActionResult ControllerPolicyLevel()
		{
			return View();
		}

		public ActionResult ControllerActionPolicyLevel()
		{
			return View();
		}
	}
}