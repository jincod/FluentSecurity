using System;
using System.Collections.Generic;
using System.Linq;
using FluentSecurity.Policy;

namespace FluentSecurity
{
	public interface IPolicyAppenderConvention
	{
		IEnumerable<Type> AppliesTo { get; }
		void BeforeAppend(IList<ISecurityPolicy> policies);
	}

	public class PolicyAppenderConvention<TSecurityPolicy> : IPolicyAppenderConvention where TSecurityPolicy : ISecurityPolicy
	{
		private readonly Type _type;
		private readonly Action<IList<ISecurityPolicy>> _beforeAppend;

		public PolicyAppenderConvention(Action<IList<ISecurityPolicy>> beforeAppend)
		{
			_type = typeof(TSecurityPolicy);
			_beforeAppend = beforeAppend;
		}

		public IEnumerable<Type> AppliesTo
		{
			get { yield return _type; }
		}

		public void BeforeAppend(IList<ISecurityPolicy> policies)
		{
			_beforeAppend.Invoke(policies);
		}
	}

	public class DefaultPolicyAppender : IPolicyAppender
	{
		private readonly IList<IPolicyAppenderConvention> _appenderConventions = new List<IPolicyAppenderConvention>();

		public DefaultPolicyAppender()
		{
			_appenderConventions.Add(new PolicyAppenderConvention<IgnorePolicy>(policies => policies.Clear()));
			_appenderConventions.Add(new PolicyAppenderConvention<DenyAnonymousAccessPolicy>(policies => policies.Clear()));
			_appenderConventions.Add(new PolicyAppenderConvention<DenyAuthenticatedAccessPolicy>(policies => policies.Clear()));
			_appenderConventions.Add(new PolicyAppenderConvention<RequireRolePolicy>(policies => policies.Clear()));
			_appenderConventions.Add(new PolicyAppenderConvention<RequireAllRolesPolicy>(policies => policies.Clear()));
		}

		public void UpdatePolicies(ISecurityPolicy securityPolicyToAdd, IList<ISecurityPolicy> policies)
		{
			if (securityPolicyToAdd == null) throw new ArgumentNullException("securityPolicyToAdd");
			if (policies == null) throw new ArgumentNullException("policies");

			var typeToAdd = securityPolicyToAdd is ILazySecurityPolicy
				? ((ILazySecurityPolicy)securityPolicyToAdd).PolicyType
				: securityPolicyToAdd.GetType();

			var conventions = _appenderConventions.Where(c => c.AppliesTo.Contains(typeToAdd));
			conventions.Each(c => c.BeforeAppend(policies));
			
			policies.Add(securityPolicyToAdd);
		}
	}
}