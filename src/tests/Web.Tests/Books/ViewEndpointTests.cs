using NUnit.Framework;
using Web.Endpoints.SiteManagement.Book;
using Web.Endpoints.SiteManagement.Book.CreateModels;
using Web.Tests.Utilities;

namespace Web.Tests.Books
{
	[TestFixture]
	public class ViewEndpointTests : RavenTestsBase
	{
		private ViewEndpoint _endpoint;

		[SetUp]
		public void CanCreate()
		{
			_endpoint = new ViewEndpoint(Session);
		}

		[Test]
		public void Get_ShouldBeLinkedToFromViewBookLinkModel()
		{
			_endpoint.Get(new ViewBookLinkModel());
		}

		[Test][Ignore]
		public void Get_GivenModelWithBooksID_ViewModelShouldContainBooksTitle()
		{
			var book = BookTestingHelper.GetBook();

			Session.Store(book);
			Session.SaveChanges();

			var model = _endpoint.Get(new ViewBookLinkModel() { Id = book.Id });

			Assert.AreEqual(book.Title, model.Title);
		}

		// should contain books genre name

		// should contain books description

		// should contain books status

		// should contain a list of the book's authors' names

		// should contain the book's ID - so we can get the image
	}
}