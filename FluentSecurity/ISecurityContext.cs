using System.Collections.Generic;

namespace FluentSecurity
{
	public interface ISecurityContext
	{
		T Data<T>() where T : class;
		void RegisterData<T>(T instance) where T : class;
		bool CurrenUserAuthenticated();
		IEnumerable<object> CurrenUserRoles();
	}
}