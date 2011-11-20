using System;
using System.Collections.Generic;
using System.Configuration;
using FluentSecurity.ServiceLocation;

namespace FluentSecurity
{
	public class SecurityContext : ISecurityContext
	{
		private readonly Dictionary<Type, object> _data;
		private readonly Func<bool> _isAuthenticated;
		private readonly Func<IEnumerable<object>> _roles;

		private SecurityContext(Func<bool> isAuthenticated, Func<IEnumerable<object>> roles)
		{
			_data = new Dictionary<Type, object>();
			_isAuthenticated = isAuthenticated;
			_roles = roles;
		}

		public T Data<T>() where T : class
		{
			var key = typeof (T);
			return _data.ContainsKey(key) ? _data[key] as T : null;
		}

		public void RegisterData<T>(T instance) where T : class
		{
			var key = typeof(T);
			if (_data.ContainsKey(key))
				throw new ArgumentException(String.Concat("An instance of {0} already exists in the data dictionary.", key.Name), "instance");
			
			_data.Add(key, instance);
		}

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
			get
			{
				return ServiceLocator.Current.Resolve<ISecurityContext>();
			}
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
					context = externalServiceLocator.Resolve(typeof(ISecurityContext)) as ISecurityContext;

				if (context == null)
				{
					if (CanCreateSecurityContextFromConfigurationExpression(configurationExpression) == false)
						throw new ConfigurationErrorsException(
							@"
							The current configuration is invalid! Before using Fluent Security you must do one of the following.
							1) Specify how to get the authentication status using GetAuthenticationStatusFrom().
							2) Register an instance of ISecurityContext in your IoC-container and register your container using ResolveServicesUsing().
							");

					context = new SecurityContext(configurationExpression.IsAuthenticated, configurationExpression.Roles);
				}
			}
			
			return context;
		}

		private static bool CanCreateSecurityContextFromConfigurationExpression(ConfigurationExpression expression)
		{
			return expression.IsAuthenticated != null;
		}
	}
}