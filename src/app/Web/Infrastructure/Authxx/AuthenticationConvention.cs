using System.Collections.Generic;
using System.Linq;
using FubuMVC.Core.Registration;

namespace Web.Infrastructure.Authxx
{
	public class AuthenticationConvention : IConfigurationAction
	{
		public void Configure(BehaviorGraph graph)
		{
			graph
			.Actions()
			.Where(b => b.InputType() != null && b.InputType().Namespace.ToLower().Contains("sitemanagement"))
			.Each(chain => chain.WrapWith<NTCodingAuthenticationBehaviour>());
		}
	}
}