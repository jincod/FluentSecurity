using FluentSecurity.Scanning;

namespace FluentSecurity
{
	public class RootConfigurationExpression : ConfigurationExpression
	{
		public RootConfigurationExpression()
		{
			CreateContext(this);
		}
	}

	public abstract class SecurityProfile : ConfigurationExpression
	{
		internal virtual void Initialize(ScannerContext context, IPolicyAppender policyAppender)
		{
			SetContext(context);
			SetPolicyAppender(policyAppender);

			Context.BeginProfileImport(GetType());
			Configure();
			Context.EndProfileImport(GetType());
		}

		public abstract void Configure();
	}
}