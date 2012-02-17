using System;
using FluentSecurity.Policy;

namespace FluentSecurity.Caching
{
	public static class CacheKeyBuilder
	{
		public static string CreateFromManifest(CacheManifest cacheManifest, string controllerName, string actionName, string policyCacheKey)
		{
			switch (cacheManifest.KeyType)
			{
				case CacheKeyType.ControllerActionPolicyType:
					return "{0}_{1}_{2}".FormatWith(controllerName, actionName, policyCacheKey);
				case CacheKeyType.ControllerPolicyType:
					return "{0}_*_{1}".FormatWith(controllerName, policyCacheKey);
				case CacheKeyType.PolicyType:
					return "*_*_{1}".FormatWith(controllerName, policyCacheKey);
				default:
					throw new ArgumentOutOfRangeException("cacheManifest");
			}
		}

		public static string CreateFromPolicy(ISecurityPolicy policy, ISecurityContext context)
		{
			var cacheKey = policy.GetType().FullName;

			var cacheKeyProvider = policy as ICacheKeyProvider;
			if (cacheKeyProvider != null)
			{
				var policyCacheKey = cacheKeyProvider.GetCacheKey(context);
				if (!String.IsNullOrWhiteSpace(policyCacheKey))
					cacheKey = policyCacheKey;
			}

			return cacheKey;
		}
	}
}