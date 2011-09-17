using System;
using Model;
using NUnit.Framework;
using Rhino.Mocks;
using Web.Endpoints;
using Web.Endpoints.HomepageModels;

namespace Web.Tests.Homepage
{
	[TestFixture]
	public class HomepageEndpointTests
	{
		private HomepageEndpoint _endpoint;
		private IHomepageContentProvider _provider;

		[SetUp]
		public void SetUp()
		{
			_provider = MockRepository.GenerateStub<IHomepageContentProvider>();
			_endpoint = new HomepageEndpoint(_provider);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldThrowExceptionIfNoHomepageContentProviderSupplied()
		{
			new HomepageEndpoint(null);
		}

		[Test]
		public void Get_ShouldBeLinkedToByHomepageLinkModel()
		{
			_endpoint.Get(new HomepageLinkModel());
		}

		[Test]
		public void Get_ShouldReturnViewModelWithHomepageContentOn()
		{
			string content = "blah";
			
			_provider.Stub(x => x.GetHomepageContent()).Return(content);

			var result = _endpoint.Get(new HomepageLinkModel());

			Assert.AreEqual(content, result.HomepageContent);
		}
	}
}