using System.Collections.Generic;
using System.Linq;
using Model;
using Model.Services;
using NUnit.Framework;
using Rhino.Mocks;
using Web.Endpoints.SiteManagement.Book;
using Web.Endpoints.SiteManagement.Book.LinkModels;
using Web.Tests.Utilities;

namespace Web.Tests.Books.SiteManagement
{
	[TestFixture]
	public class BookEndpointTests 
	{
		private BookEndpoint endpoint;
		private IBookRetriever retriever;

		// TODO - This is an example of where we want the book - but the image data is overhead we don't need

		[SetUp]
		public void CanCreate()
		{
			retriever = MockRepository.GenerateMock<IBookRetriever>();
			endpoint = new BookEndpoint(retriever);
		}

		[Test]
		public void Get_ShouldReturnModelForEachBookInSystem()
		{
			var books = CreateTwoBooks();
			retriever.Stub(r => r.GetAll()).Return(books);

			var result = endpoint.Get(new BooksLinkModel());

			books.ForEach(b => result.ShouldContainBookMOdelWithId(b.Id));
		}

			private List<Book> CreateTwoBooks()
			{
				var book1 = BookTestingHelper.GetBook();
				book1.Id = "abc";

				var book2 = BookTestingHelper.GetBook();
				book2.Id = "999";

				return new List<Book> {book1, book2};
			}

		[Test]
		public void Get_ShouldTakeABooksLinkModel()
		{
			retriever.Stub(r => r.GetAll()).Return(Enumerable.Empty<Book>());

			endpoint.Get(new BooksLinkModel());
		}

		// TODO - do we want to implement paging
	}
}