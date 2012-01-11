using System.Collections.Generic;

namespace FluentSecurity
{
	public class SecurityContextWrapper : ISecurityContext
	{
		private readonly ISecurityContext _securityContext;

		public SecurityContextWrapper(ISecurityContext innerSecurityContext)
		{
			_securityContext = innerSecurityContext;
		}
		
		public SecurityContextData Data
		{
			get { return _securityContext.Data; }
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