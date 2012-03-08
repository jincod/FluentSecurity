using System;
using System.Collections.Generic;
using System.Linq;
using FluentSecurity.Policy.ViolationHandlers.Conventions;

namespace FluentSecurity
{
	public class PolicyViolationHandlerSelector : IPolicyViolationHandlerSelector
	{
		private readonly IEnumerable<IPolicyViolationHandlerConvention> _conventions;

		public PolicyViolationHandlerSelector(IEnumerable<IPolicyViolationHandler> policyViolationHandlers)
		{
			if (policyViolationHandlers == null) throw new ArgumentNullException("policyViolationHandlers");

			var handlers = policyViolationHandlers.ToList();

			_conventions = new List<IPolicyViolationHandlerConvention>
			{
				new FindByPolicyViolationHandlerNameConvention(handlers),
				new FindDefaultPolicyViolationHandlerConvention(handlers)
			};
		}

		public IPolicyViolationHandler FindHandlerFor(PolicyViolationException exception)
		{
			IPolicyViolationHandler matchingHandler = null;
			foreach (var convention in _conventions)
			{
				matchingHandler = convention.GetHandlerFor(exception);
				if (matchingHandler != null) break;
			}
			return matchingHandler;
		}
	}
}