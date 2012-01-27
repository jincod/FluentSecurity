using System;

namespace FluentSecurity.ServiceLocation.ObjectLifeCycle
{
	// TODO: Ensure this works when unit testing (thread local storage)
	// https://github.com/structuremap/structuremap/blob/master/Source/StructureMap/Pipeline/HttpContextLifecycle.cs
	// https://github.com/structuremap/structuremap/blob/master/Source/StructureMap/Pipeline/ThreadLocalStorageLifecycle.cs
	internal class HybridHttpContextLifecycle : ILifecycle
	{
		private readonly ILifecycle _http;
		private readonly ILifecycle _nonHttp;

		public HybridHttpContextLifecycle()
		{
			_http = new HttpContextLifecycle();
			_nonHttp = new ThreadLocalStorageLifecycle();
		}

		public void EjectAll()
		{
			_http.EjectAll();
			_nonHttp.EjectAll();
		}

		public ObjectCache FindCache()
		{
			return HttpContextLifecycle.HasContext()
				? _http.FindCache()
				: _nonHttp.FindCache();
		}

		public static void Set<T>(T instance) where T : class
		{
			Set(instance, typeof(T));
		}

		public static void Set<T>(T instance, Type key) where T : class
		{
			new HybridHttpContextLifecycle().FindCache().Set(key, instance);
		}

		public static T Get<T>() where T : class
		{
			return Get<T>(typeof (T));
		}

		public static T Get<T>(Type key) where T : class
		{
			return new HybridHttpContextLifecycle().FindCache().Get(key) as T;
		}

		// TODO: Clear all when a new configuration is set/created??
		public static void DisposeAndClearAll()
		{
			new HybridHttpContextLifecycle().EjectAll();
		}
	}
}