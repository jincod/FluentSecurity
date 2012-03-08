using System.Collections.Generic;
using System.Linq;

namespace FluentSecurity.Policy.ViolationHandlers.Conventions
{
	public class FindDefaultPolicyViolationHandlerConvention : IPolicyViolationHandlerConvention
	{
		private readonly IEnumerable<IPolicyViolationHandler> _policyViolationHandlers;

		public FindDefaultPolicyViolationHandlerConvention(IEnumerable<IPolicyViolationHandler> policyViolationHandlers)
		{
			_policyViolationHandlers = policyViolationHandlers;
		}

		public IPolicyViolationHandler GetHandlerFor(PolicyViolationException exception)
		{
			return _policyViolationHandlers.SingleOrDefault(handler => handler.GetType().Name == "DefaultPolicyViolationHandler");
		}
	}
}