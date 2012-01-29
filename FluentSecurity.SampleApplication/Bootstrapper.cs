using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Principal;
using System.Web;
using System.Web.Routing;
using System.Web.Security;
using FluentSecurity.Caching;
using FluentSecurity.Policy;
using FluentSecurity.SampleApplication.Controllers;
using FluentSecurity.SampleApplication.Models;

namespace FluentSecurity.SampleApplication
{
	public static class Bootstrapper
	{
		public static ISecurityConfiguration SetupFluentSecurity()
		{
			SecurityConfigurator.Configure(configuration =>
			{
				configuration.GetAuthenticationStatusFrom(Helpers.SecurityHelper.UserIsAuthenticated);
				configuration.GetRolesFrom(Helpers.SecurityHelper.UserRoles);

				configuration.ResolveServicesUsing(type =>
				{
					var results = new List<object>();
					
					if (type == typeof(PrincipalSecurityContext))
						results.Add(new PrincipalSecurityContext(new GenericPrincipal(new GenericIdentity("Kristoffer"), null), SecurityContext.Current.Data));

					if (type == typeof(CustomSecurityContext))
						results.Add(new CustomSecurityContext(SecurityContext.Current));

					return results;
				});

				//configuration.Advanced.BuildContextUsing(innerContext => new CustomSecurityContext(innerContext));
				//configuration.Advanced.BuildContextUsing(innerContext => new PrincipalSecurityContext(new GenericPrincipal(new GenericIdentity("Kristoffer"), null), innerContext.Data));

				configuration.Advanced.CacheResults(Cache.DoNotCache);

				configuration.Advanced.BuildContextDataUsing(contextData =>
				{
					contextData.Set(HttpContext.Current.Request.AcceptTypes, "AcceptTypes");
					contextData.Set(HttpContext.Current.Server.MachineName, "MachineName");
				});

				configuration.For<HomeController>().Ignore();

				var countPolicy = new CountPolicy();
				configuration.For<CacheController>().AddPolicy(countPolicy, Cache.PerHttpRequest);
				configuration.For<CacheController>(x => x.ControllerActionPolicyLevel())
					.RemovePolicy<CountPolicy>().AddPolicy(countPolicy, Cache.DoNotCache);

				configuration.For<AccountController>(x => x.LogInAsAdministrator()).DenyAuthenticatedAccess();
				configuration.For<AccountController>(x => x.LogInAsPublisher()).DenyAuthenticatedAccess();
				configuration.For<AccountController>(x => x.LogOut()).DenyAnonymousAccess();

				configuration.For<ExampleController>(x => x.DenyAnonymousAccess()).DenyAnonymousAccess();
				configuration.For<ExampleController>(x => x.DenyAuthenticatedAccess()).DenyAuthenticatedAccess();

				configuration.For<ExampleController>(x => x.RequireAdministratorRole()).RequireRole(UserRole.Administrator);
				configuration.For<ExampleController>(x => x.RequirePublisherRole()).RequireRole(UserRole.Publisher);

				configuration.For<AdminController>().AddPolicy(new AdministratorPolicy());
				configuration.For<AdminController>(x => x.Delete()).DelegatePolicy("LocalOnlyPolicy",
					context => HttpContext.Current.Request.IsLocal
					);

				configuration.For<AdminController>(x => x.ContextWithRouteValues(0)).Ignore().AddPolicy(new RouteInfoPolicy());
				configuration.For<AdminController>(x => x.CustomContext(0)).Ignore().AddPolicy(new CustomContextPolicy());
				configuration.For<AdminController>(x => x.PrincipalContext()).Ignore().AddPolicy(new PrincipalContextPolicy());

				configuration.For<Areas.ExampleArea.Controllers.HomeController>().DenyAnonymousAccess();
				configuration.For<Areas.ExampleArea.Controllers.HomeController>(x => x.PublishersOnly()).RequireRole(UserRole.Publisher);
				configuration.For<Areas.ExampleArea.Controllers.HomeController>(x => x.AdministratorsOnly()).RequireRole(UserRole.Administrator);
			});
			return SecurityConfiguration.Current;
		}
	}

	public class CountPolicy : ISecurityPolicy
	{
		public int Executions { get; private set; }

		public PolicyResult Enforce(ISecurityContext context)
		{
			Executions++;
			Debug.Print("Executed policy {0} times", Executions);
			return PolicyResult.CreateSuccessResult(this);
		}
	}

	public class RouteInfoPolicy : ISecurityPolicy
	{
		public PolicyResult Enforce(ISecurityContext context)
		{
			var routeValues = context.Data.Get<RouteValueDictionary>();
			var id = routeValues["id"] != null ? int.Parse(routeValues["id"].ToString()) : 0;
			
			return id != 38
				? PolicyResult.CreateFailureResult(this, "Number was not 38")
				: PolicyResult.CreateSuccessResult(this);
		}
	}

	public class CustomContextPolicy : SecurityPolicyBase<CustomSecurityContext>
	{
		public override PolicyResult Enforce(CustomSecurityContext context)
		{
			return context.CustomData == "ABC" && context.Id == 38
				? PolicyResult.CreateFailureResult(this, "Access denied")
				: PolicyResult.CreateSuccessResult(this);
		}
	}

	public class PrincipalContextPolicy : SecurityPolicyBase<PrincipalSecurityContext>
	{
		public override PolicyResult Enforce(PrincipalSecurityContext context)
		{
			return context.Principal.Identity.Name != "Kristoffer"
				? PolicyResult.CreateFailureResult(this, "Access denied")
				: PolicyResult.CreateSuccessResult(this);
		}
	}

	public class CustomSecurityContext : SecurityContextWrapper
	{
		public string CustomData { get; private set; }
		public int Id { get; set; }

		public CustomSecurityContext(ISecurityContext innerSecurityContext) : base(innerSecurityContext)
		{
			var routeValues = Data.Get<RouteValueDictionary>();
			var id = routeValues["id"] != null ? int.Parse(routeValues["id"].ToString()) : 0;

			CustomData = "ABC";
			Id = id;
		}
	}

	public class PrincipalSecurityContext : ISecurityContext
	{
		public IPrincipal Principal { get; private set; }
		public SecurityContextData Data { get; private set; }

		public PrincipalSecurityContext(IPrincipal principal, SecurityContextData innerContextData)
		{
			Principal = principal;
			Data = innerContextData;
		}

		public bool CurrenUserAuthenticated()
		{
			return Principal.Identity.IsAuthenticated;
		}

		public IEnumerable<object> CurrenUserRoles()
		{
			if (!Principal.Identity.IsAuthenticated) return null;

			var userRoles = ((RolePrincipal)Principal).GetRoles();
			return userRoles;
		}
	}
}