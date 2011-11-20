using System.Web.Mvc;

namespace FluentSecurity.SampleApplication.Areas.ExampleArea
{
	public class InternalAreaAreaRegistration : AreaRegistration
	{
		public override string AreaName
		{
			get { return "InternalArea"; }
		}

		public override void RegisterArea(AreaRegistrationContext context)
		{
			context.MapRoute(
				"InternalArea_default",
				"InternalArea/{controller}/{action}/{id}",
				new { action = "Index", id = UrlParameter.Optional }
				);
		}
	}
}