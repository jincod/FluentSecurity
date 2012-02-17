using System;
using FluentSecurity.ServiceLocation.ObjectLifeCycle;

namespace FluentSecurity.Caching
{
	public static class SecurityCache<T> where T : class
	{
		public static void Store(T item, string key, Cache cache)
		{
			switch (cache)
			{
				case Cache.PerHttpRequest:
					Lifecycle<HybridHttpContextLifecycle>.Set(item, key);
					break;
				case Cache.PerHttpSession:
					Lifecycle<HttpSessionLifecycle>.Set(item, key);
					break;
				case Cache.DoNotCache:
					break;
				default:
					throw new ArgumentOutOfRangeException("cache");
			}
		}

		public static T Get(string key, Cache cache)
		{
			switch (cache)
			{
				case Cache.PerHttpRequest:
					return Lifecycle<HybridHttpContextLifecycle>.Get<T>(key);
				case Cache.PerHttpSession:
					return Lifecycle<HttpSessionLifecycle>.Get<T>(key);
				case Cache.DoNotCache:
					return null;
				default:
					throw new ArgumentOutOfRangeException("cache");
			}
		}

		public static void ClearSession()
		{
			Lifecycle<HttpSessionLifecycle>.DisposeAndClearAll();
		}
	}
}