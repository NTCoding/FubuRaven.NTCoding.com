using System;
using NUnit.Framework;
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
			endpoint = new BookEndpoint();
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

			var result = endpoint.Get();

			// invoke the get
			//result.ShouldContainBookDtoWithId(book1.Id);
			//result.ShouldContainBookDtoWithId(book2.Id);
			Assert.Inconclusive();

			// verify the result

		}

		// TODO - should take a BooksLinkModel
		
	}

	public class BookEndpoint
	{
		public object Get()
		{
			throw new NotImplementedException();
		}
	}
}