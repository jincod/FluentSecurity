using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using FluentSecurity.Caching;
using FluentSecurity.Policy;

namespace FluentSecurity
{
	public class PolicyContainer : IPolicyContainer
	{
		private readonly IList<ISecurityPolicy> _policies;
		private readonly Dictionary<Type, CacheManifest> _cacheManifest;

		public PolicyContainer(string controllerName, string actionName, IPolicyAppender policyAppender)
		{
			if (controllerName.IsNullOrEmpty())
				throw new ArgumentException("Controllername must not be null or empty!", "controllerName");

			if (actionName.IsNullOrEmpty())
				throw new ArgumentException("Actionname must not be null or empty!", "actionName");

			if (policyAppender == null)
				throw new ArgumentNullException("policyAppender");

			_policies = new List<ISecurityPolicy>();
			_cacheManifest = new Dictionary<Type, CacheManifest>();

			ControllerName = controllerName;
			ActionName = actionName;

			PolicyAppender = policyAppender;
		}

		public string ControllerName { get; private set; }
		public string ActionName { get; private set; }
		
		public IPolicyAppender PolicyAppender { get; private set; }

		public IEnumerable<PolicyResult> EnforcePolicies(ISecurityContext context)
		{
			if (_policies.Count.Equals(0))
				throw ExceptionFactory.CreateConfigurationErrorsException("You must add at least 1 policy for controller {0} action {1}.".FormatWith(ControllerName, ActionName));

			var defaultCacheLevel = SecurityConfiguration.Current.Advanced.DefaultResultsCacheLevel;

			var results = new List<PolicyResult>();
			foreach (var policy in _policies)
			{
				var policyType = policy.GetType();	
				
				var cacheLevel = defaultCacheLevel;
				var cacheKey = CacheKeyBuilder.CreateFromPolicy(policy, context);

				if (_cacheManifest.ContainsKey(policyType))
				{
					var cacheManifest = _cacheManifest[policyType];
					cacheLevel = cacheManifest.Level;
					cacheKey = CacheKeyBuilder.CreateFromManifest(cacheManifest, ControllerName, ActionName, cacheKey);
				}
				
				var result = SecurityCache<PolicyResult>.Get(cacheKey, cacheLevel);
				if (result == null)
				{
					result = policy.Enforce(context);
					SecurityCache<PolicyResult>.Store(result, cacheKey, cacheLevel);
				}
				results.Add(result);

				if (result.ViolationOccured && PolicyExecutionMode.ShouldStopOnFirstViolation)
					break;
			}

			return results.AsReadOnly();
		}

		public IPolicyContainer AddPolicy(ISecurityPolicy securityPolicy)
		{
			PolicyAppender.UpdatePolicies(securityPolicy, _policies);

			return this;
		}

		public IPolicyContainer AddPolicy(ISecurityPolicy securityPolicy, Cache level)
		{
			return AddPolicy(securityPolicy, new CacheManifest(level, CacheKeyType.ControllerActionPolicyType));
		}

		public IPolicyContainer AddPolicy(ISecurityPolicy securityPolicy, CacheManifest cacheManifest)
		{
			PolicyAppender.UpdatePolicies(securityPolicy, _policies);
			_cacheManifest[securityPolicy.GetType()] = cacheManifest;

			return this;
		}

		public IPolicyContainer RemovePolicy<TSecurityPolicy>(Func<TSecurityPolicy, bool> predicate = null) where TSecurityPolicy : ISecurityPolicy
		{
			if (predicate == null)
				predicate = x => true;

			var matchingPolicies = _policies.Where(p =>
				p is TSecurityPolicy &&
				predicate.Invoke((TSecurityPolicy)p)
				).ToList();
			
			foreach (var matchingPolicy in matchingPolicies)
				_policies.Remove(matchingPolicy);

			return this;
		}

		public IEnumerable<ISecurityPolicy> GetPolicies()
		{
			return new ReadOnlyCollection<ISecurityPolicy>(_policies);
		}
	}
}