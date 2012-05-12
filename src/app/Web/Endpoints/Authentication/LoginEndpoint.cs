using System;
using System.Net;
using FubuMVC.Core.Continuations;
using FubuMVC.Core.Http;
using FubuMVC.Core.Security;
using Web.Endpoints.SiteManagement;
using Web.Infrastructure.Authxx;

namespace Web.Endpoints.Authentication
{
	public class LoginEndpoint
	{
		private readonly IDoorStaff doorStaff;
		private readonly IAuthenticationContext authContext;
		private readonly IHttpWriter writer;

		public LoginEndpoint(IDoorStaff doorStaff, IAuthenticationContext authContext, IHttpWriter writer)
		{
			this.doorStaff = doorStaff;
			this.authContext = authContext;
			this.writer = writer;
		}

		public LoginViewModel Get(AuthRequestModel input)
		{
			if (input.MagicWord == "redsquare") return new LoginViewModel();

			writer.WriteResponseCode(HttpStatusCode.NotFound);

			return null;
		}

		public FubuContinuation Post(LoginModel loginModel)
		{
			if (doorStaff.HaveAllowedIn(loginModel.User, loginModel.Password))
			{
				authContext.ThisUserHasBeenAuthenticated(loginModel.User, true);

				return FubuContinuation.RedirectTo<SiteManagement.IndexEndpoint>(x => x.Get(new SiteManagementLinkModel()));
			}

			return FubuContinuation.EndWithStatusCode(HttpStatusCode.NotFound);
		}
	}

	public class AuthRequestModel
	{
		public string MagicWord { get; set; }
	}

	public class LoginViewModel : LoginModel
	{
	}
}