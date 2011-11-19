using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using FluentSecurity.Policy;

namespace FluentSecurity
{
	public interface ILazySecurityPolicy : ISecurityPolicy
	{
		Type PolicyType { get; }
		ISecurityPolicy Load();
	}

	internal class LazyPolicy<TSecurityPolicy> : ILazySecurityPolicy
	{
		public Type PolicyType { get; private set; }
		
		public LazyPolicy()
		{
			PolicyType = typeof(TSecurityPolicy);
		}

		public ISecurityPolicy Load()
		{
			return (ISecurityPolicy) ServiceLocation.ServiceLocator.Current.Resolve<TSecurityPolicy>();
		}

		public PolicyResult Enforce(ISecurityContext context)
		{
			return Load().Enforce(context);
		}
	}

	public class PolicyContainer : IPolicyContainer
	{
		private readonly IList<ISecurityPolicy> _policies;

		public PolicyContainer(string controllerName, string actionName, IPolicyAppender policyAppender)
		{
			if (controllerName.IsNullOrEmpty())
				throw new ArgumentException("Controllername must not be null or empty!", "controllerName");

			if (actionName.IsNullOrEmpty())
				throw new ArgumentException("Actionname must not be null or empty!", "actionName");

			if (policyAppender == null)
				throw new ArgumentNullException("policyAppender");

			_policies = new List<ISecurityPolicy>();

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

			var results = new List<PolicyResult>();
			foreach (var policy in _policies)
			{
				var result = policy.Enforce(context);
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

		public IPolicyContainer ApplyPolicy<TSecurityPolicy>() where TSecurityPolicy : ISecurityPolicy
		{
			PolicyAppender.UpdatePolicies(new LazyPolicy<TSecurityPolicy>(), _policies);

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

			var matchingLazyPolicies = _policies.Where(p =>
				p is ILazySecurityPolicy &&
				((ILazySecurityPolicy)p).PolicyType == typeof(TSecurityPolicy)
				).ToList();

			foreach (var matchingPolicy in matchingLazyPolicies)
				_policies.Remove(matchingPolicy);

			return this;
		}

		public IEnumerable<ISecurityPolicy> GetPolicies()
		{
			return new ReadOnlyCollection<ISecurityPolicy>(_policies);
		}
	}
}