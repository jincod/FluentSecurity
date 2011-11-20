using System;

namespace FluentSecurity.Policy
{
	public abstract class SecurityPolicyBase<TSecurityContext> : ISecurityPolicy where TSecurityContext : class
	{
		public PolicyResult Enforce(ISecurityContext context)
		{
			var contextType = typeof(TSecurityContext);
			var customContext = (TSecurityContext)Activator.CreateInstance(contextType, context);
			return Enforce(customContext);
		}

		public abstract PolicyResult Enforce(TSecurityContext context);
	}
}