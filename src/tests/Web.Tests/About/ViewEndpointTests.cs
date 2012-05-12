using System;
using System.Collections.Generic;
using Model.About;
using NUnit.Framework;
using Rhino.Mocks;
using Web.Endpoints.About;
using Web.Endpoints.About.LinkModels;
using Web.Endpoints.About.ViewModels;

namespace Web.Tests.About
{
	[TestFixture]
	public class ViewEndpointTests
	{
		private IAboutInfoRetriever retriever;
		private string aboutText;
		private AboutViewModel viewModel;
		private IEnumerable<string> thingsILikeUrls;

		[TestFixtureSetUp]
		public void SetUp()
		{
			retriever = MockRepository.GenerateStub<IAboutInfoRetriever>();
			
			aboutText = "This is profile info";

			thingsILikeUrls = new List<string>
			{
				"http://www.bbc.co.uk",
				"http://www.planetf1.com",
			};

			var info = new AboutInfo(aboutText, thingsILikeUrls);

			retriever.Stub(r => r.GetAboutInfo()).Return(info);

			viewModel = new ViewEndpoint(retriever).Get(new AboutLinkModel());
		}

		[Test]
		public void Fetches_about_text_and_puts_on_viewmodel()
		{
			Assert.That(viewModel.AboutText_Html, Is.EqualTo(aboutText));
		}

		[Test]
		public void Fetches_things_I_like_image_urls_and_puts_on_view_model()
		{
			Assert.That(viewModel.ThingsILikeUrls, Is.EqualTo(thingsILikeUrls));
		}
	}
}