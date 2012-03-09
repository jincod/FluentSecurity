using System.Collections.Generic;
using FluentSecurity.ServiceLocation;

namespace FluentSecurity.Policy.ViolationHandlers.Conventions
{
	public abstract class PolicyViolationHandlerConvention : IPolicyViolationHandlerConvention
	{
		private IEnumerable<IPolicyViolationHandler> _resolvedHandlers;

		protected IEnumerable<IPolicyViolationHandler> GetHandlers()
		{
			return _resolvedHandlers ?? (_resolvedHandlers = ServiceLocator.Current.ResolveAll<IPolicyViolationHandler>());
		}

		public abstract IPolicyViolationHandler GetHandlerFor(PolicyViolationException exception);
	}
}