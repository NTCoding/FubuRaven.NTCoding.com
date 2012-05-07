using System;
using System.Linq;
using DataAccessTests.Utilities;
using Model.About;
using NUnit.Framework;
using Raven.Client;

namespace DataAccessTests
{
	[TestFixture]
	public class AboutInfoUpdaterTests : RavenTestsBase
	{
		// can update the about text
		[Test]
		public void Updates_about_text()
		{
			var info = new AboutInfoDto
			           	{
			           		AboutText = "I am the about text"
			           	};

			new RavenAboutInfoUpdater(Session).Update(info);

			Session.SaveChanges();

			var fromSession = Session.Query<AboutInfo>().Single();

			Assert.That(fromSession.AboutText, Is.EqualTo(info.AboutText));

		}

		[Test][Ignore]
		public void Keeps_only_one_instance_of_about_info_with_latest_about_text()
		{
			var u = new RavenAboutInfoUpdater(Session);

			u.Update(new AboutInfoDto{AboutText = "Blah blah blah"});
			Session.SaveChanges();

			u.Update(new AboutInfoDto{AboutText = "gooblah, gooblah, gooblah"});
			Session.SaveChanges();

			var lastUpdate = "Jimmy, Jimmy, Jimmy";
			u.Update(new AboutInfoDto{AboutText = lastUpdate});
			Session.SaveChanges();

			var fromSession = Session.Query<AboutInfo>().First();
			
			Assert.That(fromSession.AboutText, Is.EqualTo(lastUpdate));

			Assert.That(Session.Query<AboutInfo>(), Is.EqualTo(1));
		}

		// can update the things I like urls
	}

	public class RavenAboutInfoUpdater : IAboutInfoUpdater
	{
		private readonly IDocumentSession session;

		public RavenAboutInfoUpdater(IDocumentSession session)
		{
			this.session = session;
		}

		public void Update(AboutInfoDto info)
		{
			session.Store(new AboutInfo(info.AboutText, null));
			
			//var currentData = session.Query<AboutInfo>().FirstOrDefault();
			
			//if (currentData != null) session.Delete(currentData);
		}
	}
}