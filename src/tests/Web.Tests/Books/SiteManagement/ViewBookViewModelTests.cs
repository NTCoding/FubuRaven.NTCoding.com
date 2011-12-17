using System.Linq;
using NUnit.Framework;
using Web.Endpoints.SiteManagement.Book.ViewModels;
using Web.Tests.Utilities;

namespace Web.Tests.Books.SiteManagement
{
	[TestFixture]
	public class ViewBookViewModelTests : RavenTestsBase
	{
		[Test]
		public void CanCreate()
		{
			new ViewBookViewModel(BookTestingHelper.GetBook());
		}

		// TODO - cannot be given a null book

		[Test]
		public void Construction_GivenABook_ShouldMapTitle()
		{
			var book = BookTestingHelper.GetBook();

			var model = new ViewBookViewModel(book);

			Assert.AreEqual(book.Title, model.Title);
		}

		[Test]
		public void Construction_GivenABook_ShouldMapGenreName()
		{
			var book = BookTestingHelper.GetBook();

			var model = new ViewBookViewModel(book);

			Assert.AreEqual(book.Genre.Name, model.GenreName);
		}

		[Test]
		public void Construction_GivenABook_ShouldMapDescription()
		{
			var book = BookTestingHelper.GetBook();

			var model = new ViewBookViewModel(book);

			Assert.AreEqual(book.Review, model.Description_Html);
		}

		[Test]
		public void Construction_GivenABook_ShouldMapAuthors()
		{
			var book = BookTestingHelper.GetBook();

			var model = new ViewBookViewModel(book);

			foreach (var author in book.Authors)
			{
				Assert.IsTrue(model.Authors.Any(a => a == author));
			}
		}

		[Test]
		public void Construction_GivenABook_ShouldHaveImageDisplayModel_WithBooksId()
		{
			var book = BookTestingHelper.GetBook();

			var model = new ViewBookViewModel(book);

			Assert.AreEqual(book.Id, model.Image.Id);
		}
	}
}
