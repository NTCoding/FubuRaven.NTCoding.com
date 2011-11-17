using System;
using NUnit.Framework;
using Web.Endpoints.SiteManagement.Book.ViewModels;
using Web.Tests.Utilities;

namespace Web.Tests.Books
{
	[TestFixture]
	public class BookDisplayModelTests
	{
		[Test]
		public void ShouldMapBooksId()
		{
			var book = BookTestingHelper.GetBook();
			book.Id = "123";

			var model = new BookDisplayModel(book);

			model.ShouldHaveId(book.Id);
		}
		
	}

	public static class BookDisplayModelAssertions
	{
		public static void ShouldHaveId (this BookDisplayModel model, string id)
		{
			if (model.Id != id) throw new Exception("Book does not have Id: " + id);
		}
	}
}