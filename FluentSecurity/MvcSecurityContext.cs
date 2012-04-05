using System.Web.Routing;

namespace FluentSecurity
{
	public class MvcSecurityContext : SecurityContextWrapper
	{
		public MvcSecurityContext(ISecurityContext innerSecurityContext) : base(innerSecurityContext)
		{
			RouteValues = innerSecurityContext.Data.RouteValues;
		}

		public RouteValueDictionary RouteValues { get; private set; }
	}
}