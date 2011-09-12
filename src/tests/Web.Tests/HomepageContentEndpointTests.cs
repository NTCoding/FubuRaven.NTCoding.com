using System;
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

		[Test]
		public void Post_GivenNewHomepageContent_ShouldSetTheSitesHomepageContent()
		{
			var newContent = "Welcome - Show some love for Fubu and Ravennnnnnn";

			var model = new HomepageContentInputModel {HomepageContent = newContent};

			_endpoint.Post(model);

			Assert.AreEqual(newContent, _homepageContentProvider.GetHomepageContent());
		}

		// TODO - once the content has been set - we need to go back to the home page

		// TODO - add some test for the get scenario - these wouldn't be covered by the specs would they?
	}

	[TestFixture]
	public class HomepageContentProviderTests : RavenTestsBase
	{
		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void SessionCannotBeNull()
		{
			new HomepageContentProvider(null);
		}

		[Test]
		public void GetHomepageContent_ShouldReturnTheCurrentHomepageContent()
		{
			var content = new HomepageContent("Welcome - I am the homepage content");
			
			Session.Store(content);
			Session.SaveChanges();

			Assert.AreEqual(content.Content, Provider.GetHomepageContent());
		}

		[Test]
		public void SetHomepageContent_ShouldSetTheCurrentHomepageContent()
		{
			var content = "this is the new content";

			Provider.SetHomepageContent(content);

			Assert.AreEqual(content, Provider.GetHomepageContent());
		}
	}

	[TestFixture]
	public class HomepageContentTest
	{
		[Test]
		public void CanCreateHomepageContent()
		{
			new HomepageContent("This is the content");
		}

		[Test]
		public void ConstructionShouldSetContent()
		{
			var content = "Content";
			var hpc = new HomepageContent(content);

			Assert.AreEqual(content, hpc.Content);
		}

		[Test]
		public void ShouldInitializeIDTo1()
		{
			var hpc = new HomepageContent("blah");

			Assert.AreEqual("1", hpc.ID);
		}
	}
}
