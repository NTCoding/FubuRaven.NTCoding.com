using System;
using NUnit.Framework;
using Web.Tests;

namespace Model.Tests
{
	[TestFixture]
	public class HomepageContentProviderTests : RavenTestsBase
	{
		private HomepageContentProvider _provider;

		[SetUp]
		public void SetUp()
		{
			_provider = new HomepageContentProvider(Session);
		}

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

			Assert.AreEqual(content.Content, _provider.GetHomepageContent());
		}

		[Test]
		public void GetHomepageContent_IfNoHomepageContent_ShouldDefaultToWelcomeMessage()
		{
			Assert.AreEqual("Welcome to NTCoding", _provider.GetHomepageContent());
		}

		[Test]
		public void SetHomepageContent_ShouldSetTheCurrentHomepageContent()
		{
			var content = "this is the new content";

			_provider.SetHomepageContent(content);

			Assert.AreEqual(content, _provider.GetHomepageContent());
		}
	}
}