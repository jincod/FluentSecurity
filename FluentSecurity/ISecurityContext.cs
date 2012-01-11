using System.Collections.Generic;

namespace FluentSecurity
{
	public interface ISecurityContext
	{
		SecurityContextData Data { get; }
		bool CurrenUserAuthenticated();
		IEnumerable<object> CurrenUserRoles();
	}
}