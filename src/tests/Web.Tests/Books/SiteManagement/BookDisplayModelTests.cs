using System;
using NUnit.Framework;
using Web.Endpoints.SiteManagement.Book.ViewModels;
using Web.Tests.Utilities;

namespace Web.Tests.Books.SiteManagement
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

		[Test]
		public void ShouldMapBooksName()
		{
			var book = BookTestingHelper.GetBook();

			var model = new BookDisplayModel(book);

			Assert.AreEqual(book.Title, model.Title);
		}

		[Test]
		public void SholdMapBooksStatus()
		{
			var book = BookTestingHelper.GetBook();

			var model = new BookDisplayModel(book);

			Assert.AreEqual(book.Status, model.Status);
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