using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FubuMVC.Core.Continuations;
using Model;
using Model.Services;
using NUnit.Framework;
using Raven.Client;
using Rhino.Mocks;
using TechTalk.SpecFlow;
using Web.Endpoints;
using Web.Endpoints.HomepageModels;
using Web.Endpoints.SiteManagement;
using Web.Endpoints.SiteManagement.HomepageContentModels;
using Web.Tests;

namespace Specs.Steps.Home_Page_Content
{
	[Binding]
	public class Editable 
	{
		private IHomepageContentProvider _contentProvider;
		private HomepageContentEndpoint _endpoint;

		[BeforeScenario()]
		public void Setup()
		{
		}

		[Given(@"I have navigated to the ""Edit Home Page"" page")]
		public void GivenIHaveNavigatedToTheEditHomePagePage()
		{
			_contentProvider = new HomepageContentProvider(null);
			_endpoint = new HomepageContentEndpoint(_contentProvider);
		}

		[Given(@"I have specified the new content as ""(.*)")]
		public void GivenIHaveSpecifiedTheNewContentAsWelcomeToNTCoding_NowGettingJiggyWithFubuAndRaven(string content)
		{
			ScenarioContext.Current["newContent"] = content;
		}

		[When(@"I confirm my new content")]
		public void WhenIConfirmMyNewContent()
		{
			var content = (String)ScenarioContext.Current["newContent"];
			ScenarioContext.Current["result"] = _endpoint.Post(new HomepageContentInputModel { HomepageContent = content });
		}

		[Then(@"I should be viewing the ""Home Page""")]
		public void ThenIShouldBeViewingTheHomePage()
		{
			var result = (FubuContinuation)ScenarioContext.Current["result"];
			result.AssertWasRedirectedTo<Web.Endpoints.IndexEndpoint>(e => e.Get(new HomepageLinkModel()));
		}

		[Then(@"I should see the following welcome content ""(.*)")]
		public void ThenIShouldSeeTheFollowingWelcomeContentWelcomeToNTCoding_NowGettingJiggyWithFubuAndRaven(string content)
		{
			var endpoint = new Web.Endpoints.IndexEndpoint(_contentProvider, null, null, null);
			var result = endpoint.Get(new HomepageLinkModel());

			Assert.AreEqual(content, result.HomepageContent);
		}
	}
}
