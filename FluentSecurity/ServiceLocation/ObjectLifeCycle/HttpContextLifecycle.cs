using System;
using System.Collections;
using System.Web;

namespace FluentSecurity.ServiceLocation.ObjectLifeCycle
{
	internal class HttpContextLifecycle : ILifecycle
	{
		public static readonly string CacheKey = "FluentSecurity-Instances";

		public void EjectAll()
		{
			FindCache().DisposeAndClear();
		}

		public ObjectCache FindCache()
		{
			var items = FindHttpDictionary();
			if (!items.Contains(CacheKey))
			{
				lock (items.SyncRoot)
				{
					if (!items.Contains(CacheKey))
					{
						var cache = new ObjectCache();
						items.Add(CacheKey, cache);
						return cache;
					}
				}
			}

			return (ObjectCache) items[CacheKey];
		}

		public static bool HasContext()
		{
			return HttpContext.Current != null;
		}

		protected virtual IDictionary FindHttpDictionary()
		{
			if (!HasContext()) throw new InvalidOperationException("HttpContext is not available.");
			return HttpContext.Current.Items;
		}
	}
}