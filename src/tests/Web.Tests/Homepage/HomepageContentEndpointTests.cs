using Model;
using Model.Services;
using NUnit.Framework;
using Rhino.Mocks;
using Web.Endpoints;
using Web.Endpoints.HomepageModels;
using Web.Endpoints.SiteManagement;
using Web.Endpoints.SiteManagement.HomepageContentModels;

namespace Web.Tests.Homepage
{
	[TestFixture]
	public class HomepageContentEndpointTests 
	{
		private HomepageContentEndpoint _endpoint;
		private IHomepageContentProvider _homepageContentProvider;

		[SetUp]
		public void SetUp()
		{
			_homepageContentProvider = MockRepository.GenerateMock<IHomepageContentProvider>();
			_endpoint = new HomepageContentEndpoint(_homepageContentProvider);
		}

		[Test]
		public void Get_ModelShouldContainCurrentHomepageContent()
		{
			string content = "Homepageeeeeeeeeeeeeeee";
			_homepageContentProvider.Stub(h => h.GetHomepageContent()).Return(content);

			var result = _endpoint.Get(new HomepageContentLinkModel());

			Assert.AreEqual(content, result.HomepageContent);
		}

		[Test]
		public void Post_GivenNewHomepageContent_ShouldSetTheSitesHomepageContent()
		{
			var newContent = "Welcome - Show some love for Fubu and Ravennnnnnn";

			var model = new HomepageContentInputModel {HomepageContent = newContent};

			_endpoint.Post(model);

			_homepageContentProvider.ShouldHaveUpdatedContentTo(newContent);
		}

		[Test]
		public void Post_ShouldRedirectToHomepage()
		{
			var model = new HomepageContentInputModel {HomepageContent = "Doesn't matter about me"};
			
			var result = _endpoint.Post(model);

			result.AssertWasRedirectedTo<Endpoints.IndexEndpoint>(c => c.Get(new HomepageLinkModel()));
		}
	}

	public static class IHomepageContentProviderTestExtensions
	{
		public static void ShouldHaveUpdatedContentTo(this IHomepageContentProvider provider, string newContent)
		{
			provider.AssertWasCalled(p => p.SetHomepageContent(newContent));
		}
	}
}
