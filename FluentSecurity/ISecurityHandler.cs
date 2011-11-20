using System.Web.Mvc;
using System.Web.Routing;

namespace FluentSecurity
{
	public interface ISecurityHandler
	{
		ActionResult HandleSecurityFor(string controllerName, string actionName, RouteValueDictionary routeValueDictionary = null);
	}
}