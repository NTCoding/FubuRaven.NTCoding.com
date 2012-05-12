using System.Net;
using FubuMVC.Core.Continuations;
using FubuMVC.Core.Security;
using Web.Endpoints.SiteManagement;
using Web.Infrastructure.Authxx;

namespace Web.Endpoints.Authentication
{
	public class AuthenticationEndpoint
	{
		private readonly IDoorStaff doorStaff;
		private readonly IAuthenticationContext authContext;

		public AuthenticationEndpoint(IDoorStaff doorStaff, IAuthenticationContext authContext)
		{
			this.doorStaff = doorStaff;
			this.authContext = authContext;
		}

		public FubuContinuation Post(LoginModel loginModel)
		{
			if (doorStaff.HaveAllowedIn(loginModel.User, loginModel.Password) & loginModel.MagicWord == "redsquare")
			{
				authContext.ThisUserHasBeenAuthenticated(loginModel.User, true);

				return FubuContinuation.RedirectTo<SiteManagement.IndexEndpoint>(x => x.Get(new SiteManagementLinkModel()));
			}

			return FubuContinuation.EndWithStatusCode(HttpStatusCode.NotFound);
		}
	}
}