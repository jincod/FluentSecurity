using FluentSecurity.Caching;

namespace FluentSecurity.Configuration
{
	public class CacheLevelExpression
	{
		public readonly CacheLevel PerHttpRequest = CacheLevel.PerHttpRequest;
		public readonly CacheLevel PerHttpSession = CacheLevel.PerHttpSession;
		public readonly CacheLevel DoNotCache = CacheLevel.DoNotCache;
		
		internal static CacheLevelExpression Instance = new CacheLevelExpression();
	}
}