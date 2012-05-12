using System;
using System.Net;
using FubuMVC.Core.Continuations;
using FubuMVC.Core.Security;
using Model.Auth;
using NUnit.Framework;
using Rhino.Mocks;
using Web.Endpoints.Authentication;
using Web.Endpoints.SiteManagement;

namespace Web.Tests.Auth
{
	[TestFixture]
	public class AuthenticationEndpointTests
	{
		private IDoorStaff doorStaff;
		private IAuthenticationContext authContext;
		private string userName;
		private AuthenticationEndpoint endpoint;

		[SetUp]
		public void SetUp()
		{
			doorStaff   = MockRepository.GenerateStub<IDoorStaff>();
			authContext = MockRepository.GenerateStub<IAuthenticationContext>();
			endpoint    = new AuthenticationEndpoint(doorStaff, authContext);
			userName    = "harryBrown";
		}

		[Test]
		public void Redirects_to_site_management_homepage_with_valid_credentials_and_magic_word()
		{
			var result = PostValidCredentials();

			result.AssertWasRedirectedTo<IndexEndpoint>(x => x.Get(new SiteManagementLinkModel()));
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

			return endpoint.Post(new LoginModel {User = userName, Password = password, MagicWord = "redsquare"});
		}

		[Test]
		public void Invalid_credentials_cause_404()
		{
			doorStaff.Stub(s => s.HaveAllowedIn("", "")).IgnoreArguments().Return(false);

			var result = endpoint.Post(new LoginModel());

			Assert.That(result._statusCode, Is.EqualTo(HttpStatusCode.NotFound));
		}

		[Test]
		public void Requires_magic_word()
		{
			doorStaff.Stub(s => s.HaveAllowedIn("", "")).IgnoreArguments().Return(true);

			var result = endpoint.Post(new LoginModel());

			Assert.That(result._statusCode, Is.EqualTo(HttpStatusCode.NotFound));
		}
	}
}