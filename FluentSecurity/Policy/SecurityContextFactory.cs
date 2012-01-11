using System;
using System.Collections.Generic;

namespace FluentSecurity.Policy
{
	public static class SecurityContextFactory
	{
		private static readonly IDictionary<Type, object> FactoryMethods = new Dictionary<Type, object>();

		public static void BuildContextUsing<TSecurityContext>(Func<ISecurityContext, TSecurityContext> factoryMethod) where TSecurityContext : class, ISecurityContext
		{
			FactoryMethods.Add(typeof(TSecurityContext), factoryMethod);
		}

		public static TSecurityContext CreateContext<TSecurityContext>(ISecurityContext innerContext) where TSecurityContext : class, ISecurityContext
		{
			TSecurityContext customContext;

			var contextType = typeof (TSecurityContext);
			if (FactoryMethods.ContainsKey(contextType))
			{
				var factoryMethod = (Func<ISecurityContext, TSecurityContext>) FactoryMethods[contextType];
				customContext = factoryMethod.Invoke(innerContext);
			}
			else
			{
				customContext = (TSecurityContext)Activator.CreateInstance(contextType, innerContext);
			}

			return customContext;
		}
	}
}