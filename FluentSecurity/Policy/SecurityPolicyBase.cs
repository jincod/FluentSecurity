using FluentSecurity.Caching;

namespace FluentSecurity.Policy
{
	public abstract class SecurityPolicyBase<TSecurityContext> : ISecurityPolicy, ICacheKeyProvider where TSecurityContext : class, ISecurityContext
	{
		public PolicyResult Enforce(ISecurityContext context)
		{
			var customContext = SecurityContextFactory.CreateContext<TSecurityContext>(context);
			return Enforce(customContext);
		}

		public abstract PolicyResult Enforce(TSecurityContext context);

		public string GetCacheKey(ISecurityContext context)
		{
			var customContext = SecurityContextFactory.CreateContext<TSecurityContext>(context);
			return GetCacheKey(customContext);
		}

		public virtual string GetCacheKey(TSecurityContext context)
		{
			return null;
		}
	}
}