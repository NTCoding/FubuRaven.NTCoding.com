using System;
using System.Collections.Generic;
using System.Linq;
using Model.About;
using NUnit.Framework;
using Rhino.Mocks;

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

		// should link back to about page
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

		public object Update(AboutUpdateModel model)
		{
			var dto = new AboutInfoDto {AboutText = model.AboutText, ThingsILikeUrls = model.ThingsILikeUrls};
			updater.Update(dto);

			return null;
		}
	}
}