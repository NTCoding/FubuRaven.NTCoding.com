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
		public static void ShouldHaveGenres_OrderedByName(this ViewBooksViewModel model)
		{
			CollectionHelper.CompareOrderOfItems(model.Genres, 
				(current, previous) => Assert.That(current.Value, Is.GreaterThan(previous.Value)));
		}

		public static void ShouldHaveReviewedBooks_OrderedByDescendingRating(this ViewBooksViewModel model)
		{
			CollectionHelper.CompareOrderOfItems(model.Books, 
				(current, previous) => Assert.That(current.Rating, Is.LessThanOrEqualTo(previous.Rating)));
		}

		public static void ShouldContainListViewFor(this ViewBooksViewModel model, IEnumerable<Book> books)
		{
			Assert.AreEqual(books.Count(), model.Books.Count(), "Different number of books");

			foreach (var book in books)
			{
				Assert.That(model.Books.Any(v => BookComparer.HasMatchingValues(v, book)), Is.True, "No match for book: " + book.Id);
			}
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