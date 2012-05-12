using System;
using FubuMVC.Core.Continuations;
using FubuMVC.Core.Security;
using NUnit.Framework;
using Rhino.Mocks;
using Web.Endpoints.SiteManagement;

namespace Web.Tests.Auth
{
	[TestFixture]
	public class AuthenticationEndpointTests
	{
		private IDoorStaff doorStaff;
		
		[SetUp]
		public void SetUp()
		{
			doorStaff = MockRepository.GenerateStub<IDoorStaff>();
		}

		[Test]
		public void Redirects_to_site_management_homepage_with_valid_credentials()
		{
			var user = "harryBrown";
			var password = "gotAShooter";

			doorStaff.Stub(s => s.WillGrantAccessTo(user, password)).Return(true);

			var result = new AuthenticationEndpoint(doorStaff).Post(new LoginModel {User = user, Password = password});

			result.AssertWasRedirectedTo<IndexEndpoint>(x => x.Get(new SiteManagementLinkModel()));
		}

		// tells fubu user has logged in

		// 404s is magic word is not part of querystring

		// 404s if credentials are invalid
	}

	public class AuthenticationEndpoint
	{
		private readonly IDoorStaff doorStaff;

		public AuthenticationEndpoint(IDoorStaff doorStaff)
		{
			this.doorStaff = doorStaff;
		}

		public FubuContinuation Post(LoginModel loginModel)
		{
			if (doorStaff.WillGrantAccessTo(loginModel.User, loginModel.Password))
			{
				return FubuContinuation.RedirectTo<IndexEndpoint>(x => x.Get(new SiteManagementLinkModel()));
			}

			return null;
		}
	}

	public class LoginModel
	{
		public string Password { get; set; }

		public string User { get; set; }
	}

	public interface IDoorStaff
	{
		bool WillGrantAccessTo(string userName, string password);
	}
}