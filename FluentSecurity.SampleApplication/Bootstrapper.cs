using System.Web;
using FluentSecurity.SampleApplication.Controllers;
using FluentSecurity.SampleApplication.Models;
using FluentSecurity.Scanning;

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

				// TODO: Test with external area and see how it feels

				configuration.Scan(x =>
				{
					x.TheCallingAssembly();
					x.LookForProfiles();
				});
			});

			//SecurityConfigurator.Configure(configuration =>
			//{
			//    configuration.GetAuthenticationStatusFrom(Helpers.SecurityHelper.UserIsAuthenticated);
			//    configuration.GetRolesFrom(Helpers.SecurityHelper.UserRoles);

			//    configuration.For<HomeController>().Ignore();

			//    configuration.For<AccountController>(x => x.LogInAsAdministrator()).DenyAuthenticatedAccess();
			//    configuration.For<AccountController>(x => x.LogInAsPublisher()).DenyAuthenticatedAccess();
			//    configuration.For<AccountController>(x => x.LogOut()).DenyAnonymousAccess();

			//    configuration.For<ExampleController>(x => x.DenyAnonymousAccess()).DenyAnonymousAccess();
			//    configuration.For<ExampleController>(x => x.DenyAuthenticatedAccess()).DenyAuthenticatedAccess();

			//    configuration.For<ExampleController>(x => x.RequireAdministratorRole()).RequireRole(UserRole.Administrator);
			//    configuration.For<ExampleController>(x => x.RequirePublisherRole()).RequireRole(UserRole.Publisher);

			//    configuration.For<AdminController>().AddPolicy(new AdministratorPolicy());
			//    configuration.For<AdminController>(x => x.Delete()).DelegatePolicy("LocalOnlyPolicy",
			//        context => HttpContext.Current.Request.IsLocal
			//        );

			//    configuration.For<Areas.ExampleArea.Controllers.HomeController>().DenyAnonymousAccess();
			//    configuration.For<Areas.ExampleArea.Controllers.HomeController>(x => x.PublishersOnly()).RequireRole(UserRole.Publisher);
			//    configuration.For<Areas.ExampleArea.Controllers.HomeController>(x => x.AdministratorsOnly()).RequireRole(UserRole.Administrator);
			//});
			return SecurityConfiguration.Current;
		}
	}

	public class AreaProfile : SecurityProfile
	{
		public override void Configure()
		{
			For<Areas.ExampleArea.Controllers.HomeController>().DenyAnonymousAccess();
			For<Areas.ExampleArea.Controllers.HomeController>(x => x.PublishersOnly()).RequireRole(UserRole.Publisher);
			For<Areas.ExampleArea.Controllers.HomeController>(x => x.AdministratorsOnly()).RequireRole(UserRole.Administrator);
			
			Scan(x =>
			{
				x.TheCallingAssembly();
				x.LookForProfiles();
			});
		}
	}

	public class WebProfile : SecurityProfile
	{
		public override void Configure()
		{
			For<HomeController>().Ignore();

			For<AccountController>(x => x.LogInAsAdministrator()).DenyAuthenticatedAccess();
			For<AccountController>(x => x.LogInAsPublisher()).DenyAuthenticatedAccess();
			For<AccountController>(x => x.LogOut()).DenyAnonymousAccess();

			For<ExampleController>(x => x.DenyAnonymousAccess()).DenyAnonymousAccess();
			For<ExampleController>(x => x.DenyAuthenticatedAccess()).DenyAuthenticatedAccess();

			For<ExampleController>(x => x.RequireAdministratorRole()).RequireRole(UserRole.Administrator);
			For<ExampleController>(x => x.RequirePublisherRole()).RequireRole(UserRole.Publisher);

			For<AdminController>().AddPolicy(new AdministratorPolicy());
			For<AdminController>(x => x.Delete()).DelegatePolicy("LocalOnlyPolicy",
				context => HttpContext.Current.Request.IsLocal
				);

			Scan(x =>
			{
				x.TheCallingAssembly();
				x.LookForProfiles();
			});
		}
	}
}