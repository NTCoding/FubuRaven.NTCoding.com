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

			var fromSession = GetAboutInfoFromSession();

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

		private AboutInfo GetAboutInfoFromSession()
		{
			return Session
				.Advanced.LuceneQuery<AboutInfo>()
				.WaitForNonStaleResults()
				.First();
		}
	}

	[TestFixture]
	public class When_updating_multiple_times : RavenTestsBase
	{
		private AboutInfo fromSession;
		private AboutInfoDto lastUpdate;

		[SetUp]
		public void SetUp()
		{
			var u = new RavenAboutInfoUpdater(Session);

			UpdateAndSave(u, "Blah blah blah", new List<string>());
			UpdateAndSave(u, "gooblah, gooblah, gooblah", new List<string> { "http://www.bbc.co.uk" });

			lastUpdate = new AboutInfoDto
							{
								AboutText = "Jimmy, Jimmy, Jimmy",
								ThingsILikeUrls = new List<string> {"http://www.planetf1.com" },
							};

			UpdateAndSave(u, lastUpdate.AboutText, lastUpdate.ThingsILikeUrls);

			fromSession = GetAboutInfoFromSession();
		}

		private void UpdateAndSave(RavenAboutInfoUpdater u, string aboutText, IEnumerable<string> thingsILikeUrls)
		{
			u.Update(new AboutInfoDto { AboutText = aboutText, ThingsILikeUrls = thingsILikeUrls});
			Session.SaveChanges();
		}

		[Test]
		public void Keeps_only_one_instance_of_about_info_with_latest_about_text()
		{
			Assert.That(Session.Query<AboutInfo>().Count(), Is.EqualTo(1));
		}

		[Test]
		public void Latest_update_to_about_text_is_kep()
		{
			Assert.That(fromSession.AboutText, Is.EqualTo(lastUpdate.AboutText));
		}

		[Test]
		public void Latest_update_to_thingsIlikeUrls_is_kept()
		{
			Assert.That(fromSession.ThingsILikeUrls, Is.EqualTo(lastUpdate.ThingsILikeUrls));
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