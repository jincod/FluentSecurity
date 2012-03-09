using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using FluentSecurity.Policy;
using FluentSecurity.Policy.ViolationHandlers.Conventions;
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

				configuration.ResolveServicesUsing(t =>
				{
					var list = new List<object>();

					// Option 1 - Add a "DefaultPolicyViolationHandler" to your container or factory
					//if (t == typeof(IPolicyViolationHandler))
					//    list.Add(new DefaultPolicyViolationHandler());
					
					if (t == typeof(CustomDefaultPolicyViolationHandler))
						list.Add(new CustomDefaultPolicyViolationHandler());

					return list;
				});

				// Option 2 - Specify what violationhandler should be used as the default
				configuration.DefaultPolicyViolationHandlerIs<CustomDefaultPolicyViolationHandler>();
				//configuration.DefaultPolicyViolationHandlerIs(() => new DefaultPolicyViolationHandler());

				// Option 3 - Conventions
				configuration.Conventions(conventions =>
				{
					// RequireRolePolicy violations will be handled by CustomDefaultPolicyViolationHandler
					conventions.ViolationsOf<RequireRolePolicy>().IsHandledBy<CustomDefaultPolicyViolationHandler>();
					conventions.ViolationsOf<RequireRolePolicy>().IsHandledBy(() => new CustomDefaultPolicyViolationHandler());

					// All violations matching the specified predicate will be handled by CustomDefaultPolicyViolationHandler
					conventions.ViolationsMatching(exception => true).IsHandledBy<CustomDefaultPolicyViolationHandler>();

					// FluentSecurity will use the violation handler returned by your convention
					conventions.Add(new CustomPolicyViolationHandlerConvention());
				});

				configuration.For<HomeController>().Ignore();

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

				configuration.For<Areas.ExampleArea.Controllers.HomeController>().DenyAnonymousAccess();
				configuration.For<Areas.ExampleArea.Controllers.HomeController>(x => x.PublishersOnly()).RequireRole(UserRole.Publisher);
				configuration.For<Areas.ExampleArea.Controllers.HomeController>(x => x.AdministratorsOnly()).RequireRole(UserRole.Administrator);
			});
			return SecurityConfiguration.Current;
		}
	}

	public class CustomPolicyViolationHandlerConvention : IPolicyViolationHandlerConvention
	{
		public IPolicyViolationHandler GetHandlerFor(PolicyViolationException exception)
		{
			return new ExceptionPolicyViolationHandler();
		}
	}

	public class CustomDefaultPolicyViolationHandler : IPolicyViolationHandler
	{
		public ActionResult Handle(PolicyViolationException exception)
		{
			return new ContentResult
			{
				Content = "Ah ah ah... Na na na... -  Custom"
			};
		}
	}

	public class DefaultPolicyViolationHandler : IPolicyViolationHandler
	{
		public ActionResult Handle(PolicyViolationException exception)
		{
			return new ContentResult
			{
				Content = "Ah ah ah..."
			};
		}
	}
}