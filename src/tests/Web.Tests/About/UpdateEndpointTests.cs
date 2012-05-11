using System;
using System.Collections.Generic;
using System.Linq;
using Model.About;
using NUnit.Framework;
using Rhino.Mocks;
using Web.Endpoints.About;
using Web.Endpoints.About.LinkModels;
using Web.Endpoints.SiteManagement.About;
using Web.Utilities;

namespace Web.Tests.About
{
	[TestFixture]
	public class UpdateEndpointTests
	{
		private IAboutInfoUpdater updater;
		private IAboutInfoRetriever retriever;

		[SetUp]
		public void SetUp()
		{
			updater   = MockRepository.GenerateStub<IAboutInfoUpdater>();
			retriever = MockRepository.GenerateStub<IAboutInfoRetriever>();
		}

		[Test]
		public void Shows_current_about_text_and_things_i_like_urls_when_viewing()
		{
			var aboutText = "Hello. I am your friend.";
			var thingsILikeUrls = new List<string>
			{
				"http://www.blah.com",
				"http://www.planetf1.com"
			};

			retriever.Stub(r => r.GetAboutInfo()).Return(new AboutInfo(aboutText, thingsILikeUrls));

			var result = new UpdateEndpoint(updater, retriever).Get(new AboutRequestModel());

			Assert.That(result.AboutText_BigText, Is.EqualTo(aboutText));
			Assert.That(result.ThingsILikeUrls.ToStrings(), Is.EqualTo(thingsILikeUrls));
		}

		[Test]
		public void Passes_on_about_text_and_things_i_like_urls_to_be_updated()
		{
			var at = "Hello, I am the about text";
			var thingsILikeUrls = new List<string>
			{
				"http://www.bbc.co.uk",
				"http://www.planetf1.com"
			};

			new UpdateEndpoint(updater, retriever).Post(new AboutUpdateModel {AboutText_BigText = at, ThingsILikeUrls = thingsILikeUrls.ToStringWrappers().ToList()});

			var info = (AboutInfoDto)updater.GetArgumentsForCallsMadeOn(u => u.Update(new AboutInfoDto()))[0][0];

			Assert.That(at, Is.EqualTo(info.AboutText));
			Assert.That(thingsILikeUrls, Is.EqualTo(info.ThingsILikeUrls));
		}

		[Test]
		public void Redirects_to_public_facing_about_page()
		{
			var result = new UpdateEndpoint(updater, retriever).Post(new AboutUpdateModel());

			result.AssertWasRedirectedTo<ViewEndpoint>(x => x.Get(new AboutLinkModel()));
		}
	}
}