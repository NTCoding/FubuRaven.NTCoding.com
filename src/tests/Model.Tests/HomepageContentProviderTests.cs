using System;
using NUnit.Framework;
using Web.Tests;

namespace Model.Tests
{
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
		public void GetHomepageContent_IfNoHomepageContent_ShouldDefaultToWelcomeMessage()
		{
			Assert.AreEqual("Welcome to NTCoding", Provider.GetHomepageContent());
		}

		[Test]
		public void SetHomepageContent_ShouldSetTheCurrentHomepageContent()
		{
			var content = "this is the new content";

			Provider.SetHomepageContent(content);

			Assert.AreEqual(content, Provider.GetHomepageContent());
		}
	}
}