using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAccessTests.Utilities;
using Model;
using Model.Services;
using NUnit.Framework;

namespace DataAccessTests
{
	// TODO - Raven tests base should only exist in this project

	[TestFixture]
	public class BookRetrieverTests : RavenTestsBase
	{
		private RavenDbBookRetriever retriever;

		[SetUp]
		public void SetUp()
		{
			retriever = new RavenDbBookRetriever(Session);
		}

		[Test]
		public void GetReviewedBooks_ShouldOnlyReturnReviewedBooks()
		{
			PopulateSessionWithBooksOfDifferentStatus();
			Session.SaveChanges();

			var reviewedFromSession = Session.Query<Book>().Where(b => b.Status == BookStatus.Reviewed);
			var reviewedFromRetriever = retriever.GetReviewedBooks();

			Assert.AreEqual(reviewedFromSession, reviewedFromRetriever);
		}

		private void PopulateSessionWithBooksOfDifferentStatus()
		{
			var currentlyReading = BookTestingHelper.CreateBooks(10, BookStatus.CurrentlyReading).ToList();
			var reviewed = BookTestingHelper.CreateBooks(10, BookStatus.Reviewed).ToList();
			var wishlist = BookTestingHelper.CreateBooks(10, BookStatus.Wishlist).ToList();
			var hidden = BookTestingHelper.CreateBooks(10, BookStatus.Hidden).ToList();

			currentlyReading.AddRange(reviewed);
			currentlyReading.AddRange(wishlist);
			currentlyReading.AddRange(hidden);

			currentlyReading.ForEach(Session.Store);
		}

		[Test]
		public void GivenGenreId_ShouldOnlyGetBooks_ForThatGenre()
		{
			PopulateSessionWithReviewedBooksForDifferentGenres();
			Session.SaveChanges();

			var genreId = Session.Query<Genre>().First().Id;

			var fromSession = Session.Query<Book>().Where(b => b.Genre.Id == genreId);
			var fromRetriever = retriever.GetReviewedBooks(genreId);

			Assert.AreEqual(fromSession, fromRetriever);
		}

		private void PopulateSessionWithReviewedBooksForDifferentGenres()
		{
			var testGenres = new[] {new Genre("genre1"), new Genre("genre2"), new Genre("genre3")};
			foreach (var g in testGenres)
			{
				Session.Store(g);
				var bs = BookTestingHelper.CreateBooks(10, BookStatus.Reviewed, genre: g);
				foreach (var book in bs)
				{
					Session.Store(book);
				}
			}
		}

		[Test]
		public void GetWishlistBooks_ShouldOnlyReturnBooks_OnTheWishlist()
		{
			Assert.Inconclusive();
		}

		[Test]
		public void GetById()
		{
			Assert.Fail();
		}

		[Test]
		public void GetAll()
		{
			Assert.Fail();
		}
	}
}
