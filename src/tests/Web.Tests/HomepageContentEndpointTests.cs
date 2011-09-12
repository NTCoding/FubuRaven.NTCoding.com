using Model;
using NUnit.Framework;
using Raven.Client.Document;
using Web.Endpoints.SiteManagement;

namespace Web.Tests
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

		[Test][Ignore]
		public void Get_ModelShouldContainCurrentHomepageContent()
		{
			// set the current homepage content
			string content = "Homepageeeeeeeeeeeeeeee";
			_homepageContentProvider.SetHomepageContent(content);

			// invoke the get action
			//var result = _endpoint.Get(new HomepageContentViewModel())

			// assert model contains the content
		}

		[Test]
		public void Post_GivenNewHomepageContent_ShouldSetTheSitesHomepageContent()
		{
			var newContent = "Welcome - Show some love for Fubu and Ravennnnnnn";

			var model = new HomepageContentInputModel {HomepageContent = newContent};

			_endpoint.Post(model);

			Assert.AreEqual(newContent, _homepageContentProvider.GetHomepageContent());
		}

		// TODO - once the content has been set - we need to go back to the home page

	}
}
