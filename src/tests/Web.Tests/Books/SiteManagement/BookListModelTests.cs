using NUnit.Framework;
using Web.Endpoints.SiteManagement.Book.ViewModels;
using Web.Tests.Utilities;

namespace Web.Tests.Books.SiteManagement
{
	[TestFixture]
	public class BookListModelTests
	{
		[Test]
		public void GivenSomeBooks_ShouldHaveDtoForEach()
		{
			var book1 = BookTestingHelper.GetBook();
			book1.Id = "123";

			var book2 = BookTestingHelper.GetBook();
			book2.Id = "345";

			var model = new BookListModel(new[] {book1, book2});

			model.ShouldContainBookMOdelWithId(book1.Id);
			model.ShouldContainBookMOdelWithId(book2.Id);
		}
	}
}