using System;
using Model;
using NUnit.Framework;
using Raven.Client;
using Web.Endpoints.SiteManagement.Book.ViewModels;
using Web.Tests.Utilities;

namespace Web.Tests.Books
{
	[TestFixture]
	public class BookEndpointTests : RavenTestsBase
	{
		private BookEndpoint endpoint;

		[SetUp]
		public void CanCreate()
		{
			endpoint = new BookEndpoint(Session);
		}

		[Test]
		public void Get_ShouldReturnModelForEachBookInSystem()
		{
			// populate the session with books
			var book1 = BookTestingHelper.GetBook();
			book1.Id = "abc";

			var book2 = BookTestingHelper.GetBook();
			book2.Id = "999";

			Session.Store(book1);
			Session.Store(book2);
			Session.SaveChanges();

			var result = endpoint.Get(new BooksLinkModel());

			result.ShouldContainBookDtoWithId(book1.Id);
			result.ShouldContainBookDtoWithId(book2.Id);
		}

		[Test]
		public void Get_ShouldTakeABooksLinkModel()
		{
			endpoint.Get(new BooksLinkModel());
		}

		// TODO - should take a BooksLinkModel

		// TODO - do we want to implement paging
		
	}

	public class BooksLinkModel
	{
	}

	public class BookEndpoint
	{
		private readonly IDocumentSession session;

		public BookEndpoint(IDocumentSession session)
		{
			this.session = session;
		}

		public BookListModel Get(BooksLinkModel booksLinkModel)
		{
			// TODO - consider paging
			var books = session.Query<Book>();

			return new BookListModel(books);
		}
	}
}