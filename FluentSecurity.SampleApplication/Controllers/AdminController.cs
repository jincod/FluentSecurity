using System.Web.Mvc;

namespace FluentSecurity.SampleApplication.Controllers
{
    public class AdminController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

    	public ActionResult Add()
    	{
    		return View();
    	}

    	public ActionResult Edit()
    	{
    		return View();
    	}

    	public ActionResult Delete()
    	{
    		return View();
    	}

		public ActionResult ContextWithRouteValues(int id)
		{
			return View();
		}

    	public ActionResult CustomContext(int id)
    	{
			return View();
    	}
    }
}
