using System;

namespace FluentSecurity.Policy.ViolationHandlers.Conventions
{
	public class LazyInstanceOfTypePolicyViolationHandlerConvention<TPolicyViolationHandler> : IPolicyViolationHandlerConvention where TPolicyViolationHandler : IPolicyViolationHandler
	{
		public Func<PolicyViolationException, bool> Predicate { get; private set; }

		public LazyInstanceOfTypePolicyViolationHandlerConvention(Func<PolicyViolationException, bool> predicate = null)
		{
			Predicate = predicate;
		}

		public IPolicyViolationHandler GetHandlerFor(PolicyViolationException exception)
		{
			var conventionApplies = Predicate == null || Predicate.Invoke(exception);
			return conventionApplies
				? (IPolicyViolationHandler) ServiceLocation.ServiceLocator.Current.Resolve<TPolicyViolationHandler>()
				: null;
		}
	}
}