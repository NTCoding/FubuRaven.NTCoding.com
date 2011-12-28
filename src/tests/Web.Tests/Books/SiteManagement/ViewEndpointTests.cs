using System.Linq;
using Model;
using NUnit.Framework;
using Web.Endpoints.SiteManagement.Book;
using Web.Endpoints.SiteManagement.Book.LinkModels;
using Web.Tests.Utilities;

namespace Web.Tests.Books.SiteManagement
{
	// TODO - Class would not exist using the endpoint testing pattern
	[TestFixture]
	public class ViewEndpointTests 
	{
		private ViewEndpoint _endpoint;

		private Book GetRandomBook()
		{
			var book = BookTestingHelper.GetBook();

			return book;
		}

		[SetUp]
		public void CanCreate()
		{
			_endpoint = new ViewEndpoint();
		}

		[Test]
		public void Get_GivenModelWithBooksID_ViewModelShouldContainBooksTitle()
		{
			var book = GetRandomBook();

			var model = _endpoint.Get(new ViewBookLinkModel { Id = book.Id });

			Assert.AreEqual(book.Title, model.Title);
		}

		[Test]
		public void Get_GivenModelWithBooksID_ViewModelSholdContainBooksGenresName()
		{
			var book = GetRandomBook();

			var model = _endpoint.Get(new ViewBookLinkModel {Id = book.Id});

			Assert.AreEqual(book.Genre.Name, model.GenreName);
		}

		[Test]
		public void Get_GivenModelWithBooksID_ViewModelShouldContainBooksDescription()
		{
			var book = GetRandomBook();

			var model = _endpoint.Get(new ViewBookLinkModel {Id = book.Id});

			Assert.AreEqual(book.Review, model.Description_Html);
		}

		[Test]
		public void Get_GivenModelWithBooksID_ViewModelShouldContainBooksStatus()
		{
			var book = GetRandomBook();

			var model = _endpoint.Get(new ViewBookLinkModel {Id = book.Id});

			Assert.AreEqual(book.Status.ToString(), model.Status);
		}

		[Test]
		public void Get_ViewModelShouldContainBooksId()
		{
			var book = GetRandomBook();

			var model = _endpoint.Get(new ViewBookLinkModel {Id = book.Id});

			Assert.AreEqual(book.Id, model.Id);
		}

		[Test]
		public void Get_GivenModelWithBooksID_ViewModelShouldContainBooksAuthorsNames()
		{
			var book = GetRandomBook();

			var model = _endpoint.Get(new ViewBookLinkModel {Id = book.Id});

			foreach (var author in book.Authors)
			{
				Assert.IsTrue(model.Authors.Any(a => a == author));
			}
		}

		// should contain the book's ID - so we can get the image
	}
}