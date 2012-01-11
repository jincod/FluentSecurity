namespace FluentSecurity.Policy
{
	public abstract class SecurityPolicyBase<TSecurityContext> : ISecurityPolicy where TSecurityContext : class, ISecurityContext
	{
		public PolicyResult Enforce(ISecurityContext context)
		{
			var customContext = SecurityContextFactory.CreateContext<TSecurityContext>(context);
			return Enforce(customContext);
		}

		public abstract PolicyResult Enforce(TSecurityContext context);
	}
}