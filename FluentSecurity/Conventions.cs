using System.Collections.Generic;
using FluentSecurity.Policy.ViolationHandlers.Conventions;

namespace FluentSecurity
{
	public class Conventions
	{
		internal readonly IList<IPolicyViolationHandlerConvention> List = new List<IPolicyViolationHandlerConvention>();

		public IEnumerable<IPolicyViolationHandlerConvention> PolicyViolationHandlerConventions
		{
			get { return List; }
		}

		public Conventions()
		{
			List.Add(new FindByPolicyViolationHandlerNameConvention());
			List.Add(new FindDefaultPolicyViolationHandlerConvention());
		}
	}
}