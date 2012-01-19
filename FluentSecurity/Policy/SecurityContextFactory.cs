using System;

namespace FluentSecurity.Policy
{
	public static class SecurityContextFactory
	{
		public static TSecurityContext CreateContext<TSecurityContext>(ISecurityContext innerContext) where TSecurityContext : class, ISecurityContext
		{
			TSecurityContext customContext = null;

			var contextType = typeof (TSecurityContext);
			
			var factoryMethods = SecurityConfiguration.Current.Advanced.ContextBuilders;
			if (factoryMethods.ContainsKey(contextType))
			{
				var factoryMethod = (Func<ISecurityContext, TSecurityContext>) factoryMethods[contextType];
				customContext = factoryMethod.Invoke(innerContext);
			}
			
			// TODO: Document feature
			// If you use your IoC to create custom contexts you are also responsible for setting context data!
			// In essence, you will need to manually set all your data or copy it from SecurityContext.Current.Data.
			// No context data will be copied automatically!!!
			if (customContext == null)
				customContext = (TSecurityContext) SecurityConfiguration.Current.ExternalServiceLocator.Resolve(contextType);

			return customContext ?? (TSecurityContext) Activator.CreateInstance(contextType, innerContext);
		}
	}
}