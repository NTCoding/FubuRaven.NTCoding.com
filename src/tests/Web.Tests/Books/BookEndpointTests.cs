using System;
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

		// TODO - do we want to implement paging
	}
}