namespace FluentSecurity.Policy.ViolationHandlers.Conventions
{
	public class LazyInstanceOfTypePolicyViolationHandlerConvention<TPolicyViolationHandler> : IPolicyViolationHandlerConvention where TPolicyViolationHandler : class, IPolicyViolationHandler
	{
		public IPolicyViolationHandler GetHandlerFor(PolicyViolationException exception)
		{
			return ServiceLocation.ServiceLocator.Current.Resolve<TPolicyViolationHandler>();
		}
	}
}