using System;
using System.Collections;
using System.Web;

namespace FluentSecurity.ServiceLocation.ObjectLifeCycle
{
	// TODO: Ensure this works when unit testing (thread local?? storage)
	internal class HttpSessionLifecycle : HttpContextLifecycle
	{
		protected override IDictionary FindHttpDictionary()
		{
			if (!HasSession()) throw new InvalidOperationException("HttpContext.Current.Session is not available.");
			return new SessionWrapper(HttpContext.Current.Session);
		}

		public bool HasSession()
		{
			return
				HttpContext.Current != null &&
				HttpContext.Current.Session != null;
		}
	}
}