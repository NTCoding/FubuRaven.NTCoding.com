using System;
using System.Collections.Generic;
using System.Linq;
using DataAccessTests.Utilities;
using Model.About;
using NUnit.Framework;

namespace DataAccessTests
{
	[TestFixture]
	public class AboutInfoUpdaterTests : RavenTestsBase
	{
		[Test]
		public void Updates_about_text()
		{
			var info = new AboutInfoDto
			           	{
			           		AboutText = "I am the about text"
			           	};

			new RavenAboutInfoUpdater(Session).Update(info);

			Session.SaveChanges();

			var fromSession = Session
				.Advanced.LuceneQuery<AboutInfo>()
				.WaitForNonStaleResults()
				.Single();

			Assert.That(fromSession.AboutText, Is.EqualTo(info.AboutText));
		}

		[Test]
		public void Updates_things_I_like_urls()
		{
			var thingsILikeUrls = new List<string>
			{
				"http://www.7digital.com",
				"http://www.autosport.com",
				"http://www.cleancoders.com"
			};

			new RavenAboutInfoUpdater(Session).Update(new AboutInfoDto { ThingsILikeUrls = thingsILikeUrls });
			Session.SaveChanges();

			var fromSession = GetAboutInfoFromSession();

			Assert.That(fromSession.ThingsILikeUrls, Is.EqualTo(thingsILikeUrls));
		}

		[Test]
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

			var fromSession = GetAboutInfoFromSession();
			
			Assert.That(fromSession.AboutText, Is.EqualTo(lastUpdate));

			Assert.That(Session.Query<AboutInfo>().Count(), Is.EqualTo(1));
		}

		private AboutInfo GetAboutInfoFromSession()
		{
			return Session
				.Advanced.LuceneQuery<AboutInfo>()
				.WaitForNonStaleResults()
				.First();
		}
	}
}