using System;
using NUnit.Framework;
using Rhino.Mocks;

namespace Web.Tests.About
{
	[TestFixture]
	public class GetEndpointTests
	{
		[Test]
		public void Fetches_about_text_and_puts_on_viewmodel()
		{
			var aboutText = "This is profile info";
			var retriever = MockRepository.GenerateStub<IAboutInfoRetriever>();
			retriever.Stub(r => r.GetAboutText()).Return(aboutText);

			var e = new GetEndpoint(retriever);

			var vm = e.Get(new AboutLinkModel());

			Assert.That(vm.AboutText, Is.EqualTo(aboutText));
		}

		// Fetches things I like image links and puts them on the view model
	}

	public interface IAboutInfoRetriever
	{
		string GetAboutText();
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
			return new AboutViewModel
			       	{
						AboutText = retriever.GetAboutText()
			       	};
		}
	}

	public class AboutViewModel
	{
		public string AboutText { get; set; }
	}
}