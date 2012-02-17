namespace FluentSecurity.ServiceLocation.ObjectLifeCycle
{
	internal static class Lifecycle<TLifecycle> where TLifecycle : ILifecycle, new()
	{
		public static TLifecycle Instance { get; set; }

		static Lifecycle()
		{
			Instance = new TLifecycle();
		}

		public static void Set<T>(T instance) where T : class
		{
			Set(instance, typeof(T).FullName);
		}

		public static void Set<T>(T instance, string key) where T : class
		{
			Instance.FindCache().Set(key, instance);
		}

		public static T Get<T>() where T : class
		{
			return Get<T>(typeof(T).FullName);
		}

		public static T Get<T>(string key) where T : class
		{
			return Instance.FindCache().Get(key) as T;
		}

		// TODO: Clear all when a new configuration is set/created??
		public static void DisposeAndClearAll()
		{
			Instance.EjectAll();
		}
	}
}