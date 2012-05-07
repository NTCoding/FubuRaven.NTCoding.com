using System;
using System.Collections.Generic;
using NUnit.Framework;
using Rhino.Mocks;

namespace Web.Tests.About
{
	[TestFixture]
	public class GetEndpointTests
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

			viewModel = new GetEndpoint(retriever).Get(new AboutLinkModel());
		}

		[Test]
		public void Fetches_about_text_and_puts_on_viewmodel()
		{
			Assert.That(viewModel.AboutText, Is.EqualTo(aboutText));
		}

		[Test]
		public void Fetches_things_I_like_image_urls_and_puts_on_view_model()
		{
			Assert.That(viewModel.ThingsILikeUrls, Is.EqualTo(thingsILikeUrls));
		}

		// Fetches things I like image links and puts them on the view model
	}

	public class AboutInfo
	{
		public AboutInfo(string aboutText, IEnumerable<string> thingsILikeUrls)
		{
			AboutText = aboutText;
			ThingsILikeUrls = thingsILikeUrls;
		}

		public string AboutText { get; private set; }
		
		public IEnumerable<string> ThingsILikeUrls { get; private set; }
	}

	public interface IAboutInfoRetriever
	{
		AboutInfo GetAboutInfo();
	}

	public class AboutLinkModel
	{
	}

	public class GetEndpoint
	{
		private readonly IAboutInfoRetriever retriever;

		public GetEndpoint(IAboutInfoRetriever retriever)
		{
			this.retriever = retriever;
		}

		public AboutViewModel Get(AboutLinkModel linkModel)
		{
			var info = retriever.GetAboutInfo();

			return new AboutViewModel
			       	{
						AboutText       = info.AboutText,
						ThingsILikeUrls = info.ThingsILikeUrls
			       	};
		}
	}

	public class AboutViewModel
	{
		public string AboutText { get; set; }

		public IEnumerable<string> ThingsILikeUrls { get; set; }
	}
}