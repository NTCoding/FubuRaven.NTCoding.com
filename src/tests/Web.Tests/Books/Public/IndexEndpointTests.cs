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
using Web.Endpoints.Books.ViewModels;
using Web.Tests.Utilities;
using IndexEndpoint = Web.Endpoints.Books.IndexEndpoint;

namespace Web.Tests.Books.Public
{
	[TestFixture]
	public class IndexEndpointTests 
	{
		private IndexEndpoint endpoint;
		private IBookRetriever bookRetriever;
		private IGenreRetriever genreRetriever;

		[SetUp]
		public void CanCreate()
		{
			bookRetriever = MockRepository.GenerateMock<IBookRetriever>();
			genreRetriever = MockRepository.GenerateMock<IGenreRetriever>();
			endpoint = new IndexEndpoint(genreRetriever, bookRetriever);
		}

		[Test]
		public void Get_ShouldTakeLinkModel()
		{
			SetUpRetrieversToReturnIrrelevantValuesSoTestsDontBreak();

			endpoint.Get(new ViewBooksLinkModel());
		}

		[Test]
		public void Get_WhenGivenNoGenreToFilterBy_ShouldListView_ForAllReviewedBooks()
		{
			genreRetriever.ReturnEmptyCollectionSoDoesntBreakTest();
			bookRetriever.ReturnEmptyWishlistBooksSoDoesntBreakTest();

			var reviewedBooks = BookTestingHelper.GetSomeReviewedBooks();
			
			bookRetriever.ReturnReviewedBooks(reviewedBooks);

			var viewModel = endpoint.Get(new ViewBooksLinkModel());

			viewModel.ShouldContainListViewFor(reviewedBooks);
		}

		[Test]
		public void Get_ShouldShowBooksOrderedByRating()
		{
			genreRetriever.ReturnEmptyCollectionSoDoesntBreakTest();
			bookRetriever.ReturnEmptyWishlistBooksSoDoesntBreakTest();

			bookRetriever.ReturnReviewedBooks(BookTestingHelper.GetSomeReviewedBooks(50));

			var viewModel = endpoint.Get(new ViewBooksLinkModel());

			viewModel.ShouldHaveReviewedBooks_OrderedByDescendingRating();
		}

		[Test]
		public void Get_VieModelShouldContainAllGenres_OrderedByName()
		{
			bookRetriever.ReturnEmptyCollectionsSoDoesntBreakTests();

			var genres = GenreTestingHelper.GetGenresWithDifferentNames();

			// TODO - extension method && Dictionary to DTOs
			genreRetriever.Stub(g => g.GetAll()).Return(genres.ToDictionary(x => x.Id, x => x.Name));

			var viewModel = endpoint.Get(new ViewBooksLinkModel());

			viewModel.ShouldHaveGenres_OrderedByName();
		}

		[Test]
		public void Get_ShouldSetSelectedGenre_WhenSpecifyingGenre_ThatExists()
		{
			bookRetriever.ReturnEmptyCollectionsSoDoesntBreakTests();

			var genre = GetGenreSimulatedToExist();

			var viewModel = endpoint.Get(new ViewBooksLinkModel {Genre = genre});

		    viewModel.ShouldHaveSelectedGenre(genre);
		}

		[Test]
		public void Get_ShouldHaveDefaultGenreMessage()
		{
			SetUpRetrieversToReturnIrrelevantValuesSoTestsDontBreak();

			var model = endpoint.Get(new ViewBooksLinkModel());

			model.ShouldHaveDefaultGenreMessage("-- All --");
		}

		[Test]
		public void Get_WhenGivenGenreId_ForGenreThatExists_ShouldReturnOnlyReviewedBooksWithThatGenre()
		{
			SetUpRetrieversToReturnIrrelevantValuesSoTestsDontBreak();

			var genreToFilter = GetGenreSimulatedToExist();

			endpoint.Get(new ViewBooksLinkModel { Genre = genreToFilter });

			bookRetriever.ShouldHaveBeenAskedToGetBooksFor(genreToFilter);
		}

		private string GetGenreSimulatedToExist()
		{
			var genreToFilter = "genres/1";

			genreRetriever.Stub(g => g.CanFindGenreWith(genreToFilter)).Return(true);

			var genres = new Dictionary<string, string> {{genreToFilter, genreToFilter}};
			
			genreRetriever.Stub(g => g.GetAll()).Return(genres);

			return genreToFilter;
		}

		private void SetUpRetrieversToReturnIrrelevantValuesSoTestsDontBreak()
		{
			bookRetriever.ReturnEmptyCollectionsSoDoesntBreakTests();
			genreRetriever.ReturnEmptyCollectionSoDoesntBreakTest();
		}

		[Test]
		public void Get_WhenGivenGenreId_ViewModelShouldHaveBooks_ReturnedFromBookRetrievers_ByGenreSearch()
		{
			genreRetriever.ReturnEmptyCollectionSoDoesntBreakTest();
			bookRetriever.ReturnEmptyWishlistBooksSoDoesntBreakTest();

			var genre = GetGenreSimulatedToExist();

			var books = BookTestingHelper.GetSomeReviewedBooks(20);

			bookRetriever.ReturnReviewedBooksForGenre(genre, books);

			var viewModel = endpoint.Get(new ViewBooksLinkModel {Genre = genre});

			viewModel.ShouldContainListViewFor(books);
		}

		[Test]
		public void Get_WhenSpecifyingGenreThatDoesntExist_ShouldShowAllReviewedBooks()
		{
			SetUpRetrieversToReturnIrrelevantValuesSoTestsDontBreak();

			genreRetriever.Stub(g => g.CanFindGenreWith(Arg<string>.Is.Anything)).Return(false);

			endpoint.Get(new ViewBooksLinkModel {Genre = "blah"});

			bookRetriever.ShouldHaveBeenAskedToGetAllReviewedBooks();
		}

		[Test]
		public void Get_ModelShouldViewOfAllWishlistBooks()
		{
			bookRetriever.ReturnEmptyReviewedBooksSoDoesntBreakTest();
			genreRetriever.ReturnEmptyCollectionSoDoesntBreakTest();

			var wishlistBooks = BookTestingHelper.GetSomeWishlistBooks();

			bookRetriever.ReturnWishlistBooks(wishlistBooks);

			var viewModel = endpoint.Get(new ViewBooksLinkModel());

			viewModel.ShouldHaveWishlistBooks(wishlistBooks);
		}
	}

	public static class IBookRetrieverTestExtensions
	{
		public static void ReturnEmptyCollectionsSoDoesntBreakTests(this IBookRetriever retriever)
		{
			ReturnEmptyReviewedBooksSoDoesntBreakTest(retriever);
			ReturnReviewedBooksForAnyGenre(retriever, new List<Book>());
			ReturnEmptyWishlistBooksSoDoesntBreakTest(retriever);
		}

		public static void ReturnEmptyReviewedBooksSoDoesntBreakTest(this IBookRetriever retriever)
		{
			ReturnReviewedBooks(retriever, new List<Book>());
		}

		private static void ReturnReviewedBooksForAnyGenre(IBookRetriever retriever, List<Book> books)
		{
			retriever.Stub(r => r.GetReviewedBooks(Arg<String>.Is.Anything)).Return(books);
		}

		public static void ReturnEmptyWishlistBooksSoDoesntBreakTest(this IBookRetriever retriever)
		{
			var books = new List<Book>();
			ReturnWishlistBooks(retriever, books);
		}

		public static void ReturnWishlistBooks(this IBookRetriever retriever, IEnumerable<Book> books)
		{
			retriever.Stub(b => b.GetWishlistBooks()).Return(books);
		}

		public static void ReturnReviewedBooks(this IBookRetriever retriever, IEnumerable<Book> reviewedBooks)
		{
			retriever.Stub(b => b.GetReviewedBooks()).Return(reviewedBooks);
		}

		public static void ReturnReviewedBooksForGenre(this IBookRetriever retriever, string genre, IEnumerable<Book> books)
		{
			retriever.Stub(r => r.GetReviewedBooks(genre)).Return(books);
		}

		public static void ReturnEmptyCurrentlyReadingSoDoesntBreakTest(this IBookRetriever bookRetriever)
		{
			bookRetriever.Stub(r => r.GetCurrentlyReading()).Return(Enumerable.Empty<Book>());
		}

		// TODO - Are we going to put these in an Assertion extensions class instead?
		public static void ShouldHaveBeenAskedToGetBooksFor(this IBookRetriever retriever, string genre)
		{
			retriever.AssertWasCalled(b => b.GetReviewedBooks(genre));
		}

		public static void ShouldHaveBeenAskedToGetAllReviewedBooks(this IBookRetriever retriever)
		{
			retriever.AssertWasCalled(b => b.GetReviewedBooks());
		}

	}

	public static class IGenreRetrieverTestExtensions
	{
		public static void ReturnEmptyCollectionSoDoesntBreakTest(this IGenreRetriever retriever)
		{
			retriever.Stub(g => g.GetAll()).Return(new Dictionary<string, string>());
		}
	}

}