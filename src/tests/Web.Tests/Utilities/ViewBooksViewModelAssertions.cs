using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Model;
using NUnit.Framework;
using Web.Endpoints.Books.ViewModels;

namespace Web.Tests.Utilities
{
	public static class ViewBooksViewModelAssertions
	{
		// TODO - refactor into collection to compare
		public static void ShouldHaveReviewedBooks_OrderedByDescendingRating(this ViewBooksViewModel model)
		{
			for (int i = 1; i < model.Books.Count(); i++)
			{
				var current = model.Books.ElementAt(i);
				var last = model.Books.ElementAt(i - 1);

				Assert.That(current.Rating, Is.LessThanOrEqualTo(last.Rating));
			}
		}

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
			       && bookListView.Title == book.Title
			       && bookListView.Rating == book.Rating;
		}

		public static void ShouldOnlyHaveReviewedBooksWith(this ViewBooksViewModel model, IEnumerable<string> ids)
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

		public static void ShouldHaveDefaultGenreMessage(this ViewBooksViewModel model, string message)
		{
			Assert.AreEqual(message, model.DefaultGenreText);
		}

		public static void ShouldHaveWishlistBooks(this ViewBooksViewModel model, IEnumerable<Book> wishlistBooks)
		{
			Assert.AreEqual(model.WishlistBooks.Count(), wishlistBooks.Count());

			foreach (var wishlistBook in wishlistBooks)
			{
				Assert.That(model.WishlistBooks.Any(b => b.Id == wishlistBook.Id));
			}
		}
	}
}