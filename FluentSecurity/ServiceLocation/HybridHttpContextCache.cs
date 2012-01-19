using System.Web;

namespace FluentSecurity.ServiceLocation
{
	// TODO: Ensure this works when unit testing
	// https://github.com/structuremap/structuremap/blob/master/Source/StructureMap/Pipeline/HttpContextLifecycle.cs
	// TODO: Store items using a better key???
	internal class HybridHttpContextCache
	{
		public static void Store<T>(T instance) where T : class
		{
			HttpContext.Current.Items.Add(typeof(T), instance);
		}

		public static T Get<T>() where T : class
		{
			var key = typeof(T);
			return HttpContext.Current.Items.Contains(key)
			       	? (T)HttpContext.Current.Items[key]
			       	: null;
		}
	}
}