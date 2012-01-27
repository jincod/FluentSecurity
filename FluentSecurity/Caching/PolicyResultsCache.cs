using System;
using FluentSecurity.ServiceLocation.ObjectLifeCycle;

namespace FluentSecurity.Caching
{
	public static class SecurityCache<T> where T : class
	{
		public static void Store(T item, Type key, CacheLevel cacheLevel)
		{
			switch (cacheLevel)
			{
				case CacheLevel.PerHttpRequest:
					HybridHttpContextLifecycle.Set(item, key);
					break;
				case CacheLevel.DoNotCache:
					break;
				default:
					throw new ArgumentOutOfRangeException("cacheLevel");
			}
		}

		public static T Get(Type key, CacheLevel cacheLevel)
		{
			switch (cacheLevel)
			{
				case CacheLevel.PerHttpRequest:
					return HybridHttpContextLifecycle.Get<T>(key);
				case CacheLevel.DoNotCache:
					return null;
				default:
					throw new ArgumentOutOfRangeException("cacheLevel");
			}
		}
	}
}