using System;
using System.Collections.Generic;
using Model;
using NUnit.Framework;
using Web.Endpoints.SiteManagement.Book;
using Web.Endpoints.SiteManagement.Book.LinkModels;
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
			var books = CreateTwoBooksAndAddThemToEmptySession();

			var result = endpoint.Get(new BooksLinkModel());

			books.ForEach(b => result.ShouldContainBookMOdelWithId(b.Id));
		}

			private List<Book> CreateTwoBooksAndAddThemToEmptySession()
			{
				var book1 = BookTestingHelper.GetBook();
				book1.Id = "abc";

				var book2 = BookTestingHelper.GetBook();
				book2.Id = "999";

				Session.Store(book1);
				Session.Store(book2);
				Session.SaveChanges();

				return new List<Book> {book1, book2};
			}

		[Test]
		public void Get_ShouldTakeABooksLinkModel()
		{
			endpoint.Get(new BooksLinkModel());
		}

		// TODO - do we want to implement paging
	}
}