﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace FluentSecurity
{
	public class PolicyViolationHandlerSelector : IPolicyViolationHandlerSelector
	{
		private readonly ISecurityConfiguration _securityConfiguration;
		private readonly IEnumerable<IPolicyViolationHandler> _policyViolationHandlers;

		public PolicyViolationHandlerSelector(ISecurityConfiguration securityConfiguration, IEnumerable<IPolicyViolationHandler> policyViolationHandlers)
		{
			if (securityConfiguration == null) throw new ArgumentNullException("securityConfiguration");
			if (policyViolationHandlers == null) throw new ArgumentNullException("policyViolationHandlers");

			_securityConfiguration = securityConfiguration;
			_policyViolationHandlers = policyViolationHandlers;
		}

		public IPolicyViolationHandler FindHandlerFor(PolicyViolationException exception)
		{
			var matchingHandler = _policyViolationHandlers.SingleOrDefault(handler => HandlerIsMatchForException(handler, exception));
			return matchingHandler;
		}

		private static bool HandlerIsMatchForException(IPolicyViolationHandler handler, PolicyViolationException exception)
		{
			var expectedHandlerName = "{0}ViolationHandler".FormatWith(exception.PolicyType.Name);
			var actualHandlerName = handler.GetType().Name;
			return expectedHandlerName == actualHandlerName;
		}
	}
}