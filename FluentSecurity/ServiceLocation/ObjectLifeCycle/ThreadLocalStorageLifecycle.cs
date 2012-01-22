using System;

namespace FluentSecurity.ServiceLocation.ObjectLifeCycle
{
	internal class ThreadLocalStorageLifecycle : ILifecycle
	{
		[ThreadStatic] 
		private static ObjectCache _cache;
		private readonly object _locker = new object();

		public void EjectAll()
		{
			FindCache().DisposeAndClear();
		}

		public ObjectCache FindCache()
		{
			EnusreCacheExists();
			return _cache;
		}

		private void EnusreCacheExists()
		{
			if (_cache != null) return;
			
			lock (_locker)
			{
				if (_cache == null)
					_cache = new ObjectCache();
			}
		}
	}
}