using System.Collections.Generic;

namespace FluentSecurity
{
	public abstract class SecurityProfile : ConfigurationExpression
	{
		internal void Initialize(List<IPolicyContainer> policyContainers, IPolicyAppender policyAppender)
		{
			PolicyContainers = policyContainers;
			SetPolicyAppender(policyAppender);
		}

		public abstract void Configure();
	}
}