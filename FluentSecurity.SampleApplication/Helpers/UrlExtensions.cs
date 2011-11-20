using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Routing;

namespace FluentSecurity.SampleApplication.Helpers
{
	public static class UrlExtensions
	{
		public static string Action<TController>(this UrlHelper urlHelper, Expression<Func<TController, object>> actionExpression) where TController : Controller
		{
			return urlHelper.Action(actionExpression, null);
		}

		public static string Action<TController>(this UrlHelper urlHelper, Expression<Func<TController, object>> actionExpression, object values) where TController : Controller
		{
			var fullControllerName = typeof(TController).GetFullControllerName();
			var actionName = actionExpression.GetActionName();
			var routeValueDictionary = new RouteValueDictionary(values);

			if (SecurityHelper.ActionIsAllowedForUser(fullControllerName, actionName, routeValueDictionary) == false)
			{
				return string.Empty;
			}

			var controllerName = typeof(TController).GetControllerName();
			return urlHelper.Action(actionName, controllerName, routeValueDictionary);
		}

		public static string AreaAction<TController>(this UrlHelper urlHelper, Expression<Func<TController, object>> actionExpression, string areaName) where TController : Controller
		{
			var fullControllerName = typeof(TController).GetFullControllerName();
			var actionName = actionExpression.GetActionName();
			var routeValueDictionary = new RouteValueDictionary { { "area", areaName } };

			if (SecurityHelper.ActionIsAllowedForUser(fullControllerName, actionName, routeValueDictionary) == false)
			{
				return string.Empty;
			}

			var controllerName = typeof(TController).GetControllerName();
			return urlHelper.Action(actionName, controllerName, routeValueDictionary);
		}
	}
}