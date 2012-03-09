using System;

namespace FluentSecurity.Policy.ViolationHandlers.Conventions
{
	public class LazyInstancePolicyViolationHandlerConvention : IPolicyViolationHandlerConvention
	{
		private readonly Func<IPolicyViolationHandler> _lazyPolicyViolationHandler;

		public LazyInstancePolicyViolationHandlerConvention(Func<IPolicyViolationHandler> lazyPolicyViolationHandler)
		{
			_lazyPolicyViolationHandler = lazyPolicyViolationHandler;
		}

		public IPolicyViolationHandler GetHandlerFor(PolicyViolationException exception)
		{
			return _lazyPolicyViolationHandler.Invoke();
		}
	}
}