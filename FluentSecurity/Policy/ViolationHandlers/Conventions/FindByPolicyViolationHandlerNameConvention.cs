using System.Linq;

namespace FluentSecurity.Policy.ViolationHandlers.Conventions
{
	public class FindByPolicyViolationHandlerNameConvention : PolicyViolationHandlerConvention
	{
		public override IPolicyViolationHandler GetHandlerFor(PolicyViolationException exception)
		{
			return GetHandlers().SingleOrDefault(handler => HandlerIsMatchForException(handler, exception));
		}

		private static bool HandlerIsMatchForException(IPolicyViolationHandler handler, PolicyViolationException exception)
		{
			var expectedHandlerName = "{0}ViolationHandler".FormatWith(exception.PolicyType.Name);
			var actualHandlerName = handler.GetType().Name;
			return expectedHandlerName == actualHandlerName;
		}
	}
}