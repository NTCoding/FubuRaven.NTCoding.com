using NUnit.Framework;
using Web.Endpoints.SiteManagement.Book.ViewModels;
using Web.Tests.Utilities;

namespace Web.Tests.Books
{
	[TestFixture]
	public class UpdateBookViewModelTests
	{
		[Test]
		public void GivenABook_ShouldMapProperties()
		{
			var book = BookTestingHelper.GetBook();

			var model = new UpdateBookViewModel(book, null);

			model.ShouldHaveDetailsFor(book);
		}
	}
}