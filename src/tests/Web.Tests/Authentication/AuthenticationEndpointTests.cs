using System.Net;
using FubuMVC.Core.Continuations;
using FubuMVC.Core.Http;
using FubuMVC.Core.Security;
using NUnit.Framework;
using Rhino.Mocks;
using Web.Endpoints.Authentication;
using Web.Endpoints.SiteManagement;
using Web.Infrastructure.Authxx;

namespace Web.Tests.Authentication
{
	[TestFixture]
	public class AuthenticationEndpointTests
	{
		private IDoorStaff doorStaff;
		private IAuthenticationContext authContext;
		private string userName;
		private LoginEndpoint endpoint;
		private IHttpWriter writer;

		[SetUp]
		public void SetUp()
		{
			doorStaff   = MockRepository.GenerateStub<IDoorStaff>();
			authContext = MockRepository.GenerateStub<IAuthenticationContext>();
			writer      = MockRepository.GenerateStub<IHttpWriter>();
			endpoint    = new LoginEndpoint(doorStaff, authContext, writer);
			userName    = "harryBrown";
		}

		[Test]
		public void Redirects_to_site_management_homepage_with_valid_credentials_and_magic_word()
		{
			var result = PostValidCredentials();

			result.AssertWasRedirectedTo<Endpoints.SiteManagement.IndexEndpoint>(x => x.Get(new SiteManagementLinkModel()));
		}

		[Test]
		public void Tells_fubu_user_has_logged_in()
		{
			PostValidCredentials();

			authContext.AssertWasCalled(a => a.ThisUserHasBeenAuthenticated(userName, true));
		}

		private FubuContinuation PostValidCredentials()
		{
			var password = "gotAShooter";

			doorStaff.Stub(s => s.HaveAllowedIn(userName, password)).Return(true);

			return endpoint.Post(new LoginModel {User = userName, Password = password});
		}

		[Test]
		public void Invalid_credentials_cause_404()
		{
			doorStaff.Stub(s => s.HaveAllowedIn("", "")).IgnoreArguments().Return(false);

			var result = endpoint.Post(new LoginModel());

			Assert.That(result._statusCode, Is.EqualTo(HttpStatusCode.NotFound));
		}

		[Test]
		public void Allows_login_attempt_with_magic_word()
		{
			endpoint.Get(new AuthRequestModel {MagicWord = "redsquare"});

			writer.AssertWasNotCalled(r => r.WriteResponseCode(HttpStatusCode.NotFound));
		}

		[Test]
		public void Will_not_allow_login_attempt_without_magic_word()
		{
			endpoint.Get(new AuthRequestModel { MagicWord = "humbagumbo" });

			writer.AssertWasCalled(r => r.WriteResponseCode(HttpStatusCode.NotFound));
		}
	}
}