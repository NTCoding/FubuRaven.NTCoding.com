using System;
using System.Collections.Generic;
using System.Linq;
using Model.About;
using NUnit.Framework;
using Rhino.Mocks;
using Web.Endpoints.About;
using Web.Endpoints.About.LinkModels;
using Web.Endpoints.SiteManagement.About;

namespace Web.Tests.About
{
	[TestFixture]
	public class UpdateEndpointTests
	{
		private IAboutInfoUpdater updater;

		[SetUp]
		public void SetUp()
		{
			updater = MockRepository.GenerateStub<IAboutInfoUpdater>();
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

			new UpdateEndpoint(updater).Update(new AboutUpdateModel {AboutText = at, ThingsILikeUrls = thingsILikeUrls});

			var info = new AboutInfoDto
			{
				AboutText       = at,
				ThingsILikeUrls = thingsILikeUrls
			};

			updater.AssertWasCalled(u => u.Update(info));
		}

		[Test]
		public void Redirects_to_public_facing_about_page()
		{
			var result = new UpdateEndpoint(updater).Update(new AboutUpdateModel());

			result.AssertWasRedirectedTo<ViewEndpoint>(x => x.Get(new AboutLinkModel()));
		}
	}
}