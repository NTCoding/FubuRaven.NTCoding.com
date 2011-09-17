using Model;
using NUnit.Framework;
using Web.Endpoints;
using Web.Endpoints.HomepageModels;
using Web.Endpoints.SiteManagement;
using Web.Endpoints.SiteManagement.HomepageContentModels;

namespace Web.Tests.Homepage
{
	[TestFixture]
	public class HomepageContentEndpointTests : RavenTestsBase
	{
		private HomepageContentEndpoint _endpoint;
		private HomepageContentProvider _homepageContentProvider;

		[SetUp]
		public void SetUp()
		{
			_homepageContentProvider = new HomepageContentProvider(Session);
			_endpoint = new HomepageContentEndpoint(_homepageContentProvider);
		}

		[Test]
		public void Get_ModelShouldContainCurrentHomepageContent()
		{
			string content = "Homepageeeeeeeeeeeeeeee";
			_homepageContentProvider.SetHomepageContent(content);

			var result = _endpoint.Get(new HomepageContentLinkModel());

			Assert.AreEqual(content, result.HomepageContent);
		}

		[Test]
		public void Post_GivenNewHomepageContent_ShouldSetTheSitesHomepageContent()
		{
			var newContent = "Welcome - Show some love for Fubu and Ravennnnnnn";

			var model = new HomepageContentInputModel {HomepageContent = newContent};

			_endpoint.Post(model);

			Assert.AreEqual(newContent, _homepageContentProvider.GetHomepageContent());
		}

		[Test]
		public void Post_ShouldRedirectToHomepage()
		{
			var model = new HomepageContentInputModel {HomepageContent = "Doesn't matter about me"};
			
			var result = _endpoint.Post(model);

			result.AssertWasRedirectedTo<HomepageEndpoint>(c => c.Get(new HomepageLinkModel()));
		}
	}
}
