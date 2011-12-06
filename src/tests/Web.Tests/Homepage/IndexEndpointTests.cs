using System;
using Model;
using Model.Services;
using NUnit.Framework;
using Rhino.Mocks;
using Web.Endpoints;
using Web.Endpoints.HomepageModels;

namespace Web.Tests.Homepage
{
	[TestFixture]
	public class IndexEndpointTests
	{
		private IndexEndpoint _endpoint;
		private IHomepageContentProvider _provider;

		[SetUp]
		public void SetUp()
		{
			_provider = MockRepository.GenerateStub<IHomepageContentProvider>();
			_endpoint = new IndexEndpoint(_provider);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldThrowExceptionIfNoHomepageContentProviderSupplied()
		{
			new IndexEndpoint(null);
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