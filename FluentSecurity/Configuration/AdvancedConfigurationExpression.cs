using System;
using System.Collections.Generic;
using FluentSecurity.Caching;

namespace FluentSecurity.Configuration
{
	public class AdvancedConfigurationExpression : IAdvancedConfiguration
	{
		public AdvancedConfigurationExpression()
		{
			ContextBuilders = new Dictionary<Type, object>();
			ContextDataBuilder = context => { };
			DefaultResultsCacheLevel = CacheLevel.DoNotCache;
		}

		public IDictionary<Type, object> ContextBuilders { get; private set; }
		public Action<SecurityContextData> ContextDataBuilder { get; private set; }
		public CacheLevel DefaultResultsCacheLevel { get; private set; }

		public void BuildContextDataUsing(Action<SecurityContextData> buildAction)
		{
			// TODO: Add null check
			ContextDataBuilder = buildAction;
		}

		public void BuildContextUsing<TSecurityContext>(Func<ISecurityContext, TSecurityContext> buildAction) where TSecurityContext : class, ISecurityContext
		{
			// TODO: Add duplication check
			ContextBuilders.Add(typeof(TSecurityContext), buildAction);
		}

		public void CacheResults(Func<CacheLevelExpression, CacheLevel> cacheLevelExpression)
		{
			DefaultResultsCacheLevel = cacheLevelExpression.Invoke(CacheLevelExpression.Instance);
		}
	}
}