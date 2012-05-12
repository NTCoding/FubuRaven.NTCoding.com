using FubuMVC.Core.Registration;
using FubuMVC.Core.Registration.Nodes;
using GenericEnumerableExtensions = System.Collections.Generic.GenericEnumerableExtensions;

namespace Web.Infrastructure.Authxx
{
	public class AuthorizationConvention : IConfigurationAction
	{
		public void Configure(BehaviorGraph graph)
		{
			GenericEnumerableExtensions.Each<BehaviorChain>(graph
				                     	.Behaviors
				                     	.Where(b => b.InputType() != null && b.InputType().Namespace.ToLower().Contains("sitemanagement")), chain => chain.Authorization.AddPolicy(typeof (NTCodingAuthorizationPolicy)));
		}
	}
}