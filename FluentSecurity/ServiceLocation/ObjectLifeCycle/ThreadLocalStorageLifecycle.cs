using System;

namespace FluentSecurity.ServiceLocation.ObjectLifeCycle
{
	internal class ThreadLocalStorageLifecycle : ILifecycle
	{
		public void EjectAll()
		{
			throw new NotImplementedException();
		}

		public ObjectCache FindCache()
		{
			throw new NotImplementedException();
		}
	}
}