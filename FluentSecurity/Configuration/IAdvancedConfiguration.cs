using System;
using System.Collections.Generic;
using FluentSecurity.Caching;

namespace FluentSecurity.Configuration
{
	public interface IAdvancedConfiguration
	{
		IDictionary<Type, object> ContextBuilders { get; }
		Action<SecurityContextData> ContextDataBuilder { get; }
		Cache DefaultResultsCacheLevel { get; }
	}
}