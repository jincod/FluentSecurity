using System.Linq;

namespace FluentSecurity.Policy.ViolationHandlers.Conventions
{
	public class FindDefaultPolicyViolationHandlerConvention : PolicyViolationHandlerConvention
	{
		public override IPolicyViolationHandler GetHandlerFor(PolicyViolationException exception)
		{
			return GetHandlers().SingleOrDefault(handler => handler.GetType().Name == "DefaultPolicyViolationHandler");
		}
	}
}