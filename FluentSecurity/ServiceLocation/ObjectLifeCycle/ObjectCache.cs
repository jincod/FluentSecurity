using System;
using System.Collections.Concurrent;

namespace FluentSecurity.ServiceLocation.ObjectLifeCycle
{
	[Serializable]
	public class ObjectCache
	{
		private readonly ConcurrentDictionary<string, object> _objects = new ConcurrentDictionary<string, object>();

		public bool Has(Type type)
		{
			return Has(CacheKeyFor(type));
		}

		public bool Has(string key)
		{
			return _objects.ContainsKey(key);
		}

		public void Eject(Type type)
		{
			if (!Has(type)) return;

			object cachedObject;
			_objects.TryRemove(CacheKeyFor(type), out cachedObject);

			TryDispose(cachedObject);
		}

		public object Get(Type type)
		{
			return Get(CacheKeyFor(type));
		}

		public object Get(string key)
		{
			return Has(key) ? _objects[key] : null;
		}

		public void Set(Type type, object value)
		{
			Set(CacheKeyFor(type), value);
		}

		public void Set(string key, object value)
		{
			if (value == null) return;
			try
			{
				_objects[key] = value;
			}
			catch (ArgumentException e)
			{
				var message = string.Format("An instance of for key {0} is already in the cache.", key);
				throw new ArgumentException(message, e);
			}
		}

		public string CacheKeyFor(Type type)
		{
			return type.FullName;
		}

		public void DisposeAndClear()
		{
			_objects.Each(@object => TryDispose(@object.Value));
			_objects.Clear();
		}

		private static void TryDispose(object cachedObject)
		{
			var disposable = cachedObject as IDisposable;
			if (disposable != null) disposable.Dispose();
		}
	}
}