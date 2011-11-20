using FluentSecurity.Scanning;

namespace FluentSecurity.SampleApplication.ExternalArea
{
	public class ExternalAreaProfile : SecurityProfile
	{
		public override void Configure()
		{
			ForAllControllers().DenyAnonymousAccess();

			Scan(x =>
			{
				x.TheCallingAssembly();
				x.LookForProfiles();
			});
		}
	}
}