namespace FluentSecurity.ServiceLocation.ObjectLifeCycle
{
	internal interface ILifecycle
	{
		void EjectAll();
		ObjectCache FindCache();
	}
}