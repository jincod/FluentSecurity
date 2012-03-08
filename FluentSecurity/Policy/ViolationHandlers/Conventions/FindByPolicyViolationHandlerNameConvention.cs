using System.Collections.Generic;
using System.Linq;

namespace FluentSecurity.Policy.ViolationHandlers.Conventions
{
	public class FindByPolicyViolationHandlerNameConvention : IPolicyViolationHandlerConvention
	{
		private readonly IEnumerable<IPolicyViolationHandler> _policyViolationHandlers;

		public FindByPolicyViolationHandlerNameConvention(IEnumerable<IPolicyViolationHandler> policyViolationHandlers)
		{
			_policyViolationHandlers = policyViolationHandlers;
		}

		public IPolicyViolationHandler GetHandlerFor(PolicyViolationException exception)
		{
			return _policyViolationHandlers.SingleOrDefault(handler => HandlerIsMatchForException(handler, exception));
		}

		private static bool HandlerIsMatchForException(IPolicyViolationHandler handler, PolicyViolationException exception)
		{
			var expectedHandlerName = "{0}ViolationHandler".FormatWith(exception.PolicyType.Name);
			var actualHandlerName = handler.GetType().Name;
			return expectedHandlerName == actualHandlerName;
		}
	}
}