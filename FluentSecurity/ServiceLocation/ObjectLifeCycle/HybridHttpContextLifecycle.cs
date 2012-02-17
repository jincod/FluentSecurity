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
	}
}