using System;
using System.Collections.Generic;
using FluentSecurity.Policy.ViolationHandlers.Conventions;

namespace FluentSecurity
{
	public class ViolationHandlerExpression
	{
		private IList<IPolicyViolationHandlerConvention> Conventions { get; set; }
		private Func<PolicyViolationException, bool> Predicate { get; set; }

		public ViolationHandlerExpression(IList<IPolicyViolationHandlerConvention> conventions, Func<PolicyViolationException, bool> predicate)
		{
			Conventions = conventions;
			Predicate = predicate;
		}

		public void IsHandledBy<TPolicyViolationHandler>() where TPolicyViolationHandler : IPolicyViolationHandler
		{
			Conventions.Add(new LazyInstanceOfTypePolicyViolationHandlerConvention<TPolicyViolationHandler>(Predicate));
		}

		public void IsHandledBy(Func<IPolicyViolationHandler> policyViolationHandler)
		{
			Conventions.Add(new LazyInstancePolicyViolationHandlerConvention(policyViolationHandler, Predicate));
		}
	}
}