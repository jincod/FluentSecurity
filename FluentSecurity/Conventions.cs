using System;
using System.Collections.Generic;
using FluentSecurity.Policy;
using FluentSecurity.Policy.ViolationHandlers.Conventions;

namespace FluentSecurity
{
	public class Conventions
	{
		private readonly IList<IPolicyViolationHandlerConvention> _policyViolationHandlerConventions = new List<IPolicyViolationHandlerConvention>();

		public IEnumerable<IPolicyViolationHandlerConvention> PolicyViolationHandlerConventions
		{
			get { return _policyViolationHandlerConventions; }
		}

		public Conventions()
		{
			_policyViolationHandlerConventions.Add(new FindByPolicyViolationHandlerNameConvention());
			_policyViolationHandlerConventions.Add(new FindDefaultPolicyViolationHandlerConvention());
		}

		public ViolationHandlerExpression ViolationsOf<TSecurityPolicy>() where TSecurityPolicy : ISecurityPolicy
		{
			return new ViolationHandlerExpression(_policyViolationHandlerConventions, exception => exception.PolicyType == typeof(TSecurityPolicy));
		}

		public ViolationHandlerExpression ViolationsMatching(Func<PolicyViolationException, bool> predicate)
		{
			return new ViolationHandlerExpression(_policyViolationHandlerConventions, predicate);
		}

		public void Add(IPolicyViolationHandlerConvention convention)
		{
			_policyViolationHandlerConventions.Add(convention);
		}

		public void Remove(IPolicyViolationHandlerConvention convention)
		{
			_policyViolationHandlerConventions.Remove(convention);
		}
	}
}