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
					HybridHttpContextLifecycle.Set(item, key);
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
					return HybridHttpContextLifecycle.Get<T>(key);
				case Cache.DoNotCache:
					return null;
				default:
					throw new ArgumentOutOfRangeException("cache");
			}
		}
	}
}