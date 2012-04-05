using System;
using System.Collections.Generic;

namespace FluentSecurity.Configuration
{
	public class AdvancedConfigurationExpression : IAdvancedConfiguration
	{
		public AdvancedConfigurationExpression()
		{
			ContextBuilders = new Dictionary<Type, object>();
		}

		public IDictionary<Type, object> ContextBuilders { get; private set; }

		public void BuildContextUsing<TSecurityContext>(Func<ISecurityContext, TSecurityContext> buildAction) where TSecurityContext : class, ISecurityContext
		{
			// TODO: Add duplication check
			ContextBuilders.Add(typeof(TSecurityContext), buildAction);
		}
	}
}