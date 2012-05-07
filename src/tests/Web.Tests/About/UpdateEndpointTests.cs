using System;
using System.Collections.Generic;
using System.Linq;
using FubuMVC.Core.Continuations;
using Model.About;
using NUnit.Framework;
using Rhino.Mocks;
using Web.Endpoints.About;
using Web.Endpoints.About.LinkModels;

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

	public interface IAboutInfoUpdater
	{
		void Update(AboutInfoDto info);
	}

	public struct AboutInfoDto
	{
		public string AboutText { get; set; }

		public IEnumerable<string> ThingsILikeUrls { get; set; }
	}

	public class AboutUpdateModel
	{
		public string AboutText { get; set; }

		public List<string> ThingsILikeUrls { get; set; }
	}

	public class UpdateEndpoint
	{
		private readonly IAboutInfoUpdater updater;

		public UpdateEndpoint(IAboutInfoUpdater updater)
		{
			this.updater = updater;
		}

		public FubuContinuation Update(AboutUpdateModel model)
		{
			var dto = new AboutInfoDto {AboutText = model.AboutText, ThingsILikeUrls = model.ThingsILikeUrls};
			updater.Update(dto);

			return FubuContinuation.RedirectTo<ViewEndpoint>(x => x.Get(new AboutLinkModel()));
		}
	}
}