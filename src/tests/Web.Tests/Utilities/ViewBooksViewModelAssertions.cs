using System.Collections.Generic;
using System.Linq;
using Model;
using NUnit.Framework;
using Web.Endpoints.Books.ViewModels;

namespace Web.Tests.Utilities
{
	public static class ViewBooksViewModelAssertions
	{
		public static void ShouldContainListViewFor(this ViewBooksViewModel model, IEnumerable<Book> books)
		{
			Assert.AreEqual(books.Count(), model.Books.Count(), "Different number of books");

			foreach (var book in books)
			{
				Assert.That(model.Books.Any(v => HasMatchingValues(v, book)), Is.True, "No match for book: " + book.Id);
			}
		}

		private static bool HasMatchingValues(BookListView bookListView, Book book)
		{
			return bookListView.Id == book.Id
			       && bookListView.Image.Id == book.Id
			       && bookListView.Title == book.Title;
		}

		public static void ShouldOnlyHaveBooksWith(this ViewBooksViewModel model, IEnumerable<string> ids)
		{
			Assert.AreEqual(ids.Count(), model.Books.Count());

			foreach (var id in ids)
			{
				Assert.That(model.Books.Any(b => b.Id == id), "No id for: " + id);
			}
		}

		public static void ShouldHaveSelectedGenre(this Web.Endpoints.Books.ViewModels.ViewBooksViewModel model, string genre)
		{
			Assert.AreEqual(genre, model.SelectedGenre);
		}
	}
}