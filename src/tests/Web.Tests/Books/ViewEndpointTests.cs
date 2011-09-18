using NUnit.Framework;
using Web.Endpoints.SiteManagement.Book.CreateModels;
using Web.Endpoints.SiteManagement.Book.View;

namespace Web.Tests.Books
{
	[TestFixture]
	public class ViewEndpointTests
	{
		private ViewEndpoint _endpoint;

		[SetUp]
		public void CanCreate()
		{
			_endpoint = new ViewEndpoint();
		}

		[Test]
		public void Get_ShouldBeLinkedToFromViewBookLinkModel()
		{
			_endpoint.Get(new ViewBookLinkModel());
		}
	}
}