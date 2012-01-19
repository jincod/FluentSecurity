using System;
using System.Collections.Concurrent;

namespace FluentSecurity.ServiceLocation.ObjectLifeCycle
{
	[Serializable]
	public class ObjectCache
	{
		private readonly ConcurrentDictionary<Type, object> _objects = new ConcurrentDictionary<Type, object>();

		public bool Has(Type key)
		{
			return _objects.ContainsKey(key);
		}

		public void Eject(Type key)
		{
			if (!Has(key)) return;

			object cachedObject;
			_objects.TryRemove(key, out cachedObject);

			TryDispose(cachedObject);
		}

		public object Get(Type key)
		{
			return Has(key) ? _objects[key] : null;
		}

		public void Set(Type key, object value)
		{
			if (value == null) return;
			try
			{
				_objects[key] = value;
			}
			catch (ArgumentException e)
			{
				var message = string.Format("An instance of type {0} is already in the cache.", key.FullName);
				throw new ArgumentException(message, e);
			}
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