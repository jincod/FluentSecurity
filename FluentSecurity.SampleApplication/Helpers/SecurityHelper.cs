using System.Collections.Generic;
using System.Linq;
using System.Web.Routing;

namespace FluentSecurity.SampleApplication.Helpers
{
	public static class SecurityHelper
	{
		public static bool ActionIsAllowedForUser(string controllerName, string actionName, RouteValueDictionary routeValueDictionary)
		{
			var configuration = SecurityConfiguration.Current; 
			var policyContainer = configuration.PolicyContainers.GetContainerFor(controllerName, actionName);
			if (policyContainer != null)
			{
				var context = SecurityContext.Current;
				context.Data.Set(routeValueDictionary, true);
				var results = policyContainer.EnforcePolicies(context);
				return results.All(x => x.ViolationOccured == false);
			}
			return true;
		}

		public static bool UserIsAuthenticated()
		{
			var currentUser = SessionContext.Current.User;
			return currentUser != null;
		}

		public static IEnumerable<object> UserRoles()
		{
			var currentUser = SessionContext.Current.User;
			return currentUser != null ? currentUser.Roles.Cast<object>().ToArray() : null;
		}
	}
}