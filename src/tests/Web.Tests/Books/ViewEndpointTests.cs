using System.Linq;
using Model;
using NUnit.Framework;
using Web.Endpoints.SiteManagement.Book;
using Web.Endpoints.SiteManagement.Book.CreateModels;
using Web.Endpoints.SiteManagement.Book.LinkModels;
using Web.Tests.Utilities;

namespace Web.Tests.Books
{
	[TestFixture]
	public class ViewEndpointTests : RavenTestsBase
	{
		private ViewEndpoint _endpoint;

		private Book GetBookFromSession()
		{
			var book = BookTestingHelper.GetBook();

			Session.Store(book);
			Session.SaveChanges();
			return book;
		}

		[SetUp]
		public void CanCreate()
		{
			_endpoint = new ViewEndpoint(Session);
		}

		[Test]
		public void Get_GivenModelWithBooksID_ViewModelShouldContainBooksTitle()
		{
			var book = GetBookFromSession();

			var model = _endpoint.Get(new ViewBookLinkModel { Id = book.Id });

			Assert.AreEqual(book.Title, model.Title);
		}

		[Test]
		public void Get_GivenModelWithBooksID_ViewModelSholdContainBooksGenresName()
		{
			var book = GetBookFromSession();

			var model = _endpoint.Get(new ViewBookLinkModel {Id = book.Id});

			Assert.AreEqual(book.Genre.Name, model.GenreName);
		}

		[Test]
		public void Get_GivenModelWithBooksID_ViewModelShouldContainBooksDescription()
		{
			var book = GetBookFromSession();

			var model = _endpoint.Get(new ViewBookLinkModel {Id = book.Id});

			Assert.AreEqual(book.Description, model.Description);
		}

		[Test]
		public void Get_GivenModelWithBooksID_ViewModelShouldContainBooksStatus()
		{
			var book = GetBookFromSession();

			var model = _endpoint.Get(new ViewBookLinkModel {Id = book.Id});

			Assert.AreEqual(book.Status.ToString(), model.Status);
		}

		[Test]
		public void Get_ViewModelShouldContainBooksId()
		{
			var book = GetBookFromSession();

			var model = _endpoint.Get(new ViewBookLinkModel {Id = book.Id});

			Assert.AreEqual(book.Id, model.Id);
		}

		[Test]
		public void Get_GivenModelWithBooksID_ViewModelShouldContainBooksAuthorsNames()
		{
			var book = GetBookFromSession();

			var model = _endpoint.Get(new ViewBookLinkModel {Id = book.Id});

			foreach (var author in book.Authors)
			{
				Assert.IsTrue(model.Authors.Any(a => a == author));
			}
		}

		// should contain the book's ID - so we can get the image
	}
}