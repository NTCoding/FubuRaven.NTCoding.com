using FubuMVC.Core.Runtime;
using FubuMVC.Core.Security;

namespace Web.Infrastructure.Authxx
{
	public class NTCodingAuthorizationPolicy : IAuthorizationPolicy
	{
		public AuthorizationRight RightsFor(IFubuRequest request)
		{
			return AuthorizationRight.Deny;
		}
	}
}