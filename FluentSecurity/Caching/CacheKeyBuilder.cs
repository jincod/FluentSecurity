using System;

namespace FluentSecurity.Caching
{
	public static class CacheKeyBuilder
	{
		public static string Create(CacheManifest cacheManifest, string controllerName, string actionName, Type policyType)
		{
			var policyTypeKey = policyType.FullName;
			switch (cacheManifest.KeyType)
			{
				case CacheKeyType.ControllerActionPolicyType:
					return "{0}_{1}_{2}".FormatWith(controllerName, actionName, policyTypeKey);
				case CacheKeyType.ControllerPolicyType:
					return "{0}_*_{1}".FormatWith(controllerName, policyTypeKey);
				case CacheKeyType.PolicyType:
					return "*_*_{1}".FormatWith(controllerName, policyTypeKey);
				default:
					throw new ArgumentOutOfRangeException("cacheKey");
			}
		}
	}
}