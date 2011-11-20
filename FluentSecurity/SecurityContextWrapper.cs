using System.Collections.Generic;

namespace FluentSecurity
{
	public class SecurityContextWrapper : ISecurityContext
	{
		private readonly ISecurityContext _securityContext;

		public SecurityContextWrapper(ISecurityContext securityContext)
		{
			_securityContext = securityContext;
		}

		public T Data<T>() where T : class
		{
			return _securityContext.Data<T>();
		}

		public void RegisterData<T>(T instance) where T : class
		{
			_securityContext.RegisterData(instance);
		}

		public bool CurrenUserAuthenticated()
		{
			return _securityContext.CurrenUserAuthenticated();
		}

		public IEnumerable<object> CurrenUserRoles()
		{
			return _securityContext.CurrenUserRoles();
		}
	}
}