using System;
using System.Web.Mvc;
using FluentSecurity.ServiceLocation;

namespace FluentSecurity
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
	public class HandleSecurityAttribute : ActionFilterAttribute
	{
		public ISecurityHandler SecurityHandler { get; private set; }

		public HandleSecurityAttribute() : this(ServiceLocator.Current.Resolve<ISecurityHandler>()) {}

		public HandleSecurityAttribute(ISecurityHandler securityHandler)
		{
			SecurityHandler = securityHandler;
		}

		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			var actionName = filterContext.ActionDescriptor.ActionName;
			var controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerType.FullName;
			var routeValueDictionary = filterContext.RouteData.Values;

			var securityContext = SecurityContext.Current;
			securityContext.Data.RouteValues = routeValueDictionary;

			var overrideResult = SecurityHandler.HandleSecurityFor(controllerName, actionName, securityContext);
			if (overrideResult != null) filterContext.Result = overrideResult;
		}
	}
}