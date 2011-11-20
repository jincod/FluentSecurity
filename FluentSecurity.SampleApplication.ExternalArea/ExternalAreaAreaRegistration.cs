using System.Web.Mvc;

namespace FluentSecurity.SampleApplication.ExternalArea
{
	public class ExternalAreaAreaRegistration : AreaRegistration
	{
		public override string AreaName
		{
			get
			{
				return "ExternalArea";
			}
		}

		public override void RegisterArea(AreaRegistrationContext context)
		{
			context.MapRoute(
				"ExternalArea_default",
				"ExternalArea/{controller}/{action}/{id}",
				new { action = "Index", id = UrlParameter.Optional }
			);
		}
	}
}
