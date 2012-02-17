namespace FluentSecurity.Caching
{
	public interface ICacheKeyProvider
	{
		string GetCacheKey(ISecurityContext context);
	}
}