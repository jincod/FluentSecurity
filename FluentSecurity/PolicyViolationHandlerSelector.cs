using System;
using System.Collections.Generic;
using System.Linq;
using FluentSecurity.Policy.ViolationHandlers.Conventions;

namespace FluentSecurity
{
	public class PolicyViolationHandlerSelector : IPolicyViolationHandlerSelector
	{
		private readonly IEnumerable<IPolicyViolationHandlerConvention> _conventions;

		public PolicyViolationHandlerSelector(IEnumerable<IPolicyViolationHandler> policyViolationHandlers) {}

		public PolicyViolationHandlerSelector(Func<IEnumerable<IPolicyViolationHandlerConvention>> conventions)
		{
			if (conventions == null) throw new ArgumentNullException("conventions");
			
			_conventions = conventions.Invoke();
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