using System;
using System.Collections.Generic;
using System.Linq;
using DataAccessTests.Utilities;
using Model.About;
using NUnit.Framework;
using Raven.Client;

namespace DataAccessTests
{
	[TestFixture]
	public class AboutInfoRetrieverTests : RavenTestsBase
	{
		[Test]
		public void Retrieves_about_text()
		{
			var aboutText = "Hello sir. How are you?";

			var info = new AboutInfo(aboutText, Enumerable.Empty<string>());

			Session.Store(info);
			Session.SaveChanges();

			var retriever = new RavenAboutInfoRetriever(Session);
			var returnedInfo = retriever.GetAboutInfo();

			Assert.That(returnedInfo.AboutText, Is.EqualTo(aboutText));
		}

		[Test]
		public void Returns_empty_string_when_no_about_text_exists()
		{
			var retriever = new RavenAboutInfoRetriever(Session);
			var returnedInfo = retriever.GetAboutInfo();

			Assert.That(returnedInfo.AboutText, Is.EqualTo(string.Empty));
		}

		[Test]
		public void Retrieves_things_i_like_image_urls()
		{
			var thingsILikeUrls = new List<string>
			{
				"http://www.bbc.co.uk",
				"http://www.planetf1.com"
			};

			var info = new AboutInfo("", thingsILikeUrls);

			Session.Store(info);
			Session.SaveChanges();

			var returnedInfo = new RavenAboutInfoRetriever(Session).GetAboutInfo();

			Assert.That(returnedInfo.ThingsILikeUrls, Is.EqualTo(thingsILikeUrls));
		}

		[Test]
		public void Defaulats_things_i_like_urls_to_emtpy_collection()
		{
			var retriever = new RavenAboutInfoRetriever(Session);
			var returnedInfo = retriever.GetAboutInfo();

			Assert.That(returnedInfo.ThingsILikeUrls.Count(), Is.EqualTo(0));
		}
	}

	public class RavenAboutInfoRetriever : IAboutInfoRetriever
	{
		private readonly IDocumentSession session;

		public RavenAboutInfoRetriever(IDocumentSession session)
		{
			this.session = session;
		}

		public AboutInfo GetAboutInfo()
		{
			return session.Query<AboutInfo>()
			       	.SingleOrDefault() ?? new AboutInfo(string.Empty, Enumerable.Empty<string>());

		}
	}
}