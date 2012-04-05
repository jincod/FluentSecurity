using System;
using System.Collections.Generic;
using System.Configuration;
using System.Dynamic;
using FluentSecurity.ServiceLocation;
using FluentSecurity.ServiceLocation.ObjectLifeCycle;

namespace FluentSecurity
{
	public class SecurityContext : ISecurityContext
	{
		private readonly Func<bool> _isAuthenticated;
		private readonly Func<IEnumerable<object>> _roles;

		private SecurityContext(Func<bool> isAuthenticated, Func<IEnumerable<object>> roles)
		{
			// TODO: Document that the factory method will be called once per created security context
			// TODO: Move creation to suitable place and pass SecurityContextData to the constructor
			//var contextData = new SecurityContextData();
			//SecurityConfiguration.Current.Advanced.ContextDataBuilder.Invoke(contextData);
			Data = new ExpandoObject();

			_isAuthenticated = isAuthenticated;
			_roles = roles;
		}

		public dynamic Data { get; private set; }

		public bool CurrenUserAuthenticated()
		{
			return _isAuthenticated();
		}

		public IEnumerable<object> CurrenUserRoles()
		{
			return _roles != null ? _roles() : null;
		}

		public static ISecurityContext Current
		{
			get { return ServiceLocator.Current.Resolve<ISecurityContext>(); }
		}

		internal static ISecurityContext CreateFrom(ISecurityConfiguration configuration)
		{
			ISecurityContext context = null;

			var securityConfiguration = configuration as SecurityConfiguration;
			if (securityConfiguration != null)
			{
				var configurationExpression = securityConfiguration.Expression;
				var externalServiceLocator = configurationExpression.ExternalServiceLocator;
				if (externalServiceLocator != null)
					context = externalServiceLocator.Resolve(typeof (ISecurityContext)) as ISecurityContext;

				if (context == null)
				{
					if (CanCreateSecurityContextFromConfigurationExpression(configurationExpression) == false)
						throw new ConfigurationErrorsException(
							@"
							The current configuration is invalid! Before using Fluent Security you must do one of the following.
							1) Specify how to get the authentication status using GetAuthenticationStatusFrom().
							2) Register an instance of ISecurityContext in your IoC-container and register your container using ResolveServicesUsing().
							");

					context = GetFromCacheOrCreateContext(configurationExpression);
				}
			}

			return context;
		}

		private static bool CanCreateSecurityContextFromConfigurationExpression(ConfigurationExpression expression)
		{
			return expression.IsAuthenticated != null;
		}

		private static SecurityContext GetFromCacheOrCreateContext(ConfigurationExpression configurationExpression)
		{
			// TODO: Ensure that this context object is cached and only created once per Http request.

			var context = HybridHttpContextLifecycle.Get<SecurityContext>();
			if (context != null) return context;

			context = new SecurityContext(configurationExpression.IsAuthenticated, configurationExpression.Roles);
			HybridHttpContextLifecycle.Set(context);
			return context;
		}
	}
}