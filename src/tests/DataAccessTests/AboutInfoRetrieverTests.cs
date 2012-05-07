using System;
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
		// retrieves about text
		[Test]
		public void Retriever_about_text()
		{
			var aboutText = "Hello sir. How are you?";

			var info = new AboutInfo(aboutText, Enumerable.Empty<string>());

			Session.Store(info);
			Session.SaveChanges();

			var retriever = new RavenAboutInfoRetriever(Session);
			var returnedInfo = retriever.GetAboutInfo();

			Assert.That(returnedInfo.AboutText, Is.EqualTo(aboutText));
		}

		// retriever image url
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
						   .SingleOrDefault();

		}
	}
}