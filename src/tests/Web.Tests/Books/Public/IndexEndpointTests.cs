using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using Model.Services;
using NUnit.Framework;
using Rhino.Mocks;
using Web.Endpoints;
using Web.Endpoints.Books;
using Web.Endpoints.Books.LinkModels;
using Web.Tests.Utilities;
using IndexEndpoint = Web.Endpoints.Books.IndexEndpoint;

namespace Web.Tests.Books.Public
{
	// TODO - move the raven tests base into a common testing project
	//        and remove the dependency between testing projects
	[TestFixture]
	public class IndexEndpointTests : RavenTestsBase
	{
		private IndexEndpoint endpoint;
		private IBookRetriever bookRetriever;

		[SetUp]
		public void CanCreate()
		{
			bookRetriever = MockRepository.GenerateMock<IBookRetriever>();
			// TODO - so this is an integration test - how are we defining when to verify calls and when to use components?
			//        I think using a component and not mocking the interface still verifies the behaviour of this component
			//        it tells us that this unit works correctly with an implementation of the interface that also works
			endpoint = new IndexEndpoint(new RavenDbGenreRetriever(Session), bookRetriever);
		}

		[Test]
		public void Get_ShouldTakeLinkModel()
		{
			bookRetriever.ReturnEmptyCollectionsSoDoesntBreakTests();

			endpoint.Get(new ViewBooksLinkModel());
		}

		[Test]
		public void Get_WhenGivenNoGenreToFilterBy_ShouldListView_ForAllReviewedBooks()
		{
			var books = BookTestingHelper.GetSomeReviewedBooks();

			bookRetriever.ReturnReviewedBooksOrderedByRating(books);

			bookRetriever.ReturnEmptyCollectionsSoDoesntBreakTests();

			var viewModel = endpoint.Get(new ViewBooksLinkModel());

			viewModel.ShouldContainListViewFor(books);
		}

		// TODO - need a test for the book retriever that only returns reviewed books

		//[Test]
		//public void Get_ShouldOrderBooksByRating()
		//{
		//    GetBooksWithMixedStatusRetrieverWillReturn().ToList();

		//    endpoint.Get(new ViewBooksLinkModel());

		//    bookRetriever.AssertWasCalled(r => r.GetReviewedBooksOrderedByRating());
		//}

		// TODO - should ask retriever to get books ordered by rating

		//[Test]
		//public void Get_VieModelShouldContainAllGenresInSession_OrderedByName()
		//{
		//    var genres = GenreTestingHelper.GetGenresFromSession(Session);

		//    var viewModel = endpoint.Get(new ViewBooksLinkModel());

		//    genres.OrderBy(g => g.Name).ShouldMatch(viewModel.Genres);
		//}

		// TODO - view model should have all the genres returned from genre retriever

		//[Test]
		//public void Get_ShouldSetSelectedGenre()
		//{
		//    var genres = GenreTestingHelper.GetGenresFromSession(Session);

		//    var viewModel = endpoint.Get(new ViewBooksLinkModel {Genre = genres.First().Id});

		//    viewModel.ShouldHaveSelectedGenre(genres.First().Name);
		//}

		// TODO - given genre that retriever will return

		[Test]
		public void Get_ShouldHaveDefaultGenreMessage()
		{
			bookRetriever.ReturnEmptyCollectionsSoDoesntBreakTests();

			var model = endpoint.Get(new ViewBooksLinkModel());

			model.ShouldHaveDefaultGenreMessage("-- All --");
		}

		//[Test]
		//public void Get_WhenGivenGenreId_ShouldReturnOnlyReviewedBooksWithThatGenre()
		//{
		//    var genreToFilter = "genres/1";
			
		//    var idsForBookWithGenre1 = PutBooksInSessionWithDifferentGenresAndGetIdsForBooksWithGenre(genreToFilter);

		//    var result = endpoint.Get(new ViewBooksLinkModel {Genre = genreToFilter});
			
		//    result.ShouldOnlyHaveReviewedBooksWith(idsForBookWithGenre1);
		//}

		// TODO - should ask retriever to get by id, should show that genre on the view model

		//[Test]
		//public void Get_WhenSpecifyingGenreThatDoesntExist_ShouldShowAllReviewedBooks()
		//{
		//    var books = GetBooksWithMixedStatusRetrieverWillReturn().ToList();

		//    var model = endpoint.Get(new ViewBooksLinkModel {Genre = "Genres/DoesNotExist"});

		//    model.ShouldContainListViewFor(books.Where(b => b.Status == BookStatus.Reviewed));
		//}

		// TODO - when retriever says cannot find genre - should query to get all books

		//[Test]
		//public void Get_ModelShouldViewOfAllWishlistBooks()
		//{
		//    var books = GetWishlistBooksRetrieverWillReturn();

		//    var model = endpoint.Get(new ViewBooksLinkModel());

		//    model.ShouldHaveWishlistBooks(books.Where(b => b.Status == BookStatus.Wishlist));
		//}

		// TODO - should ask to get wishlist books and model should have them all

		//private void GetBooksWithMixedStatusRetrieverWillReturn()
		//{
		//    var book1 = BookTestingHelper.GetBook(id: "Books/001", status: BookStatus.CurrentlyReading);
		//    var book2 = BookTestingHelper.GetBook(id: "Books/002", status: BookStatus.CurrentlyReading);
		//    var book3 = BookTestingHelper.GetBook(id: "Books/003", status: BookStatus.Reviewed);
		//    var book4 = BookTestingHelper.GetBook(id: "Books/004", status: BookStatus.Reviewed);
		//    var book5 = BookTestingHelper.GetBook(id: "Books/005", status: BookStatus.Wishlist);
		//    var book6 = BookTestingHelper.GetBook(id: "Books/006", status: BookStatus.Wishlist);
		//    var book7 = BookTestingHelper.GetBook(id: "Books/007", status: BookStatus.Hidden);
		//    var book8 = BookTestingHelper.GetBook(id: "Books/008", status: BookStatus.Hidden);

		//    var books = new[] {book1, book2, book3, book4, book5, book6, book7, book8};

		//    bookRetriever.Stub(r => r.)
		//}

		private string[] PutBooksInSessionWithDifferentGenresAndGetIdsForBooksWithGenre(string genre)
		{
			var genre1 = new Model.Genre(genre);
			Session.Store(genre1);
			
			var book1 = BookTestingHelper.GetBook(genre: genre1, id: "Books/1");
			Session.Store(book1);

			var book2 = BookTestingHelper.GetBook(genre: genre1, id: "Books/2");
			Session.Store(book2);

			var genre2 = new Model.Genre("2");
			Session.Store(genre2);

			var book3 = BookTestingHelper.GetBook(genre: genre2);
			Session.Store(book3);

			Session.SaveChanges();

			return new[] { book1.Id, book2.Id };
		}

		//private IEnumerable<Book> PopulateAndGetAllBooksExistingFromSession()
		//{
		//    yield return GetBookFromSessionWithId("Books/001", BookStatus.CurrentlyReading);
		//    yield return GetBookFromSessionWithId("Books/002", BookStatus.CurrentlyReading);
		//    yield return GetBookFromSessionWithId("Books/003", BookStatus.Reviewed);
		//    yield return GetBookFromSessionWithId("Books/004", BookStatus.Reviewed);
		//    yield return GetBookFromSessionWithId("Books/005", BookStatus.Wishlist);
		//    yield return GetBookFromSessionWithId("Books/006", BookStatus.Wishlist);
		//    yield return GetBookFromSessionWithId("Books/007", BookStatus.Hidden);
		//    yield return GetBookFromSessionWithId("Books/008", BookStatus.Hidden);
		//    Session.SaveChanges();
		//}

		//private Book GetBookFromSessionWithId(string id, BookStatus status)
		//{
		//    var book1 = BookTestingHelper.GetBook(status: status);
		//    book1.Id = id;
		//    Session.Store(book1);
		//    return book1;
		//}
	}

	public static class IBookRetrieverTestExtensions
	{
		public static void ReturnEmptyCollectionsSoDoesntBreakTests(this IBookRetriever retriever)
		{
			ReturnReviewedBooksOrderedByRating(retriever, new List<Book>());
			ReturnEmptyWishlistBooksSoDoesntBreakTest(retriever);
		}

		public static void ReturnEmptyWishlistBooksSoDoesntBreakTest(IBookRetriever retriever)
		{
			retriever.Stub(b => b.GetWishlistBooks()).Return(new List<Book>());
		}

		public static void ReturnReviewedBooksOrderedByRating(this IBookRetriever retriever, IEnumerable<Book> reviewedBooks)
		{
			retriever.Stub(b => b.GetReviewedBooksOrderedByRating()).Return(reviewedBooks);
		}
	}
}