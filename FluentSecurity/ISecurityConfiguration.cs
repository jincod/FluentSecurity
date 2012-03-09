using System.Collections.Generic;

namespace FluentSecurity
{
	public interface ISecurityConfiguration
	{
		Conventions Conventions { get; }
		IEnumerable<IPolicyContainer> PolicyContainers { get; }
		ISecurityServiceLocator ExternalServiceLocator { get; }
		bool IgnoreMissingConfiguration { get; }
		string WhatDoIHave();
	}
}