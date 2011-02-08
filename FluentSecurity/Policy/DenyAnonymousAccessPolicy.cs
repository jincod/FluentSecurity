namespace FluentSecurity.Policy
{
	public class DenyAnonymousAccessPolicy : ISecurityPolicy
	{
		public void Enforce(bool isAuthenticated, object[] roles)
		{
			if (isAuthenticated == false)
			{
				throw new FluentSecurityException<DenyAnonymousAccessPolicy>("Anonymous access denied");
			}
		}

		public object[] RolesRequired
		{
			get { return null; }
		}
	}
}