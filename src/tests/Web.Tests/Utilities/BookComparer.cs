using Model;
using Web.Endpoints.Books.ViewModels;

namespace Web.Tests.Utilities
{
	public static class BookComparer
	{
		public static bool HasMatchingValues(BookListView bookListView, Book book)
		{
			return bookListView.Id == book.Id
			       && bookListView.Image.Id == book.Id
			       && bookListView.Title == book.Title
			       && bookListView.Rating == book.Rating;
		}
	}
}