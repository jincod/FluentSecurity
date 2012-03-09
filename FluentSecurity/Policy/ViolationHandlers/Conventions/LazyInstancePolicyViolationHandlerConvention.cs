using System;

namespace FluentSecurity.Policy.ViolationHandlers.Conventions
{
	public class LazyInstancePolicyViolationHandlerConvention : IPolicyViolationHandlerConvention
	{
		private readonly Func<IPolicyViolationHandler> _lazyPolicyViolationHandler;

		public Func<PolicyViolationException, bool> Predicate { get; private set; }

		public LazyInstancePolicyViolationHandlerConvention(Func<IPolicyViolationHandler> lazyPolicyViolationHandler, Func<PolicyViolationException, bool> predicate = null)
		{
			_lazyPolicyViolationHandler = lazyPolicyViolationHandler;
			Predicate = predicate;
		}

		public IPolicyViolationHandler GetHandlerFor(PolicyViolationException exception)
		{
			var conventionApplies = Predicate == null || Predicate.Invoke(exception);
			return conventionApplies
				? _lazyPolicyViolationHandler.Invoke()
				: null;
		}
	}
}