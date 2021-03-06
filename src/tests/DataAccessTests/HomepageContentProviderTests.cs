﻿using System;
using DataAccessTests.Utilities;
using Model;
using Model.Services;
using NUnit.Framework;

namespace DataAccessTests
{
	[TestFixture]
	public class HomepageContentProviderTests : RavenTestsBase
	{
		private HomepageContentProvider provider;

		[SetUp]
		public void SetUp()
		{
			provider = new HomepageContentProvider(Session);
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

			Assert.AreEqual((object) content.Content, provider.GetHomepageContent());
		}

		[Test]
		public void GetHomepageContent_IfNoHomepageContent_ShouldDefaultToWelcomeMessage()
		{
			Assert.AreEqual("Welcome to NTCoding", provider.GetHomepageContent());
		}

		[Test]
		public void SetHomepageContent_ShouldSetTheCurrentHomepageContent()
		{
			var content = "this is the new content";

			provider.SetHomepageContent(content);

			Assert.AreEqual(content, provider.GetHomepageContent());
		}
	}
}