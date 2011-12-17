using System.Collections.Generic;
using System.Linq;
using Model;
using Model.Services;
using NUnit.Framework;
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

		[SetUp]
		public void CanCreate()
		{
			// TODO - so this is an integration test - how are we defining when to verify calls and when to use components?
			//        I think using a component and not mocking the interface still verifies the behaviour of this component
			//        it tells us that this unit works correctly with an implementation of the interface that also works
			endpoint = new IndexEndpoint(Session, new RavenDbGenreRetriever(Session));
		}

		[Test]
		public void Get_ShouldTakeLinkModel()
		{
			endpoint.Get(new ViewBooksLinkModel());
		}
		
		[Test]
		public void Get_WhenGivenNoGenreToFilterBy_ShouldListView_ForAllExistingBooks()
		{
			var books = PopulateAndGetAllBooksExistingFromSession().ToList();

			var result = endpoint.Get(new ViewBooksLinkModel());

			result.ShouldContainListViewFor(books);
		}

		[Test]
		public void Get_VieModelShouldContainAllGenresInSession_OrderedByName()
		{
			var genres = GenreTestingHelper.GetGenresFromSession(Session);

			var viewModel = endpoint.Get(new ViewBooksLinkModel());

			genres.OrderBy(g => g.Name).ShouldMatch(viewModel.Genres);
		}

		[Test]
		public void Get_ShouldSetSelectedGenre()
		{
			var genres = GenreTestingHelper.GetGenresFromSession(Session);

			var viewModel = endpoint.Get(new ViewBooksLinkModel {Genre = genres.First().Id});

			viewModel.ShouldHaveSelectedGenre(genres.First().Name);
		}

		[Test]
		public void Get_ShouldHaveDefaultGenreMessage()
		{
			var model = endpoint.Get(new ViewBooksLinkModel());

			model.ShouldHaveDefaultGenreMessage("-- All --");
		}

		// TODO - genres should be ordered

		// TODO - test convention - because endpoints have specific models - the models need no tests. Covered by the endpoints
		[Test]
		public void Get_WhenGivenGenreId_ShouldReturnOnlyBooksWithThatGenre()
		{
			var genreToFilter = "genres/1";
			
			var idsForBookWithGenre1 = PutBooksInSessionWithDifferentGenresAndGetIdsForBooksWithGenre(genreToFilter);

			var result = endpoint.Get(new ViewBooksLinkModel {Genre = genreToFilter});
			
			result.ShouldOnlyHaveBooksWith(idsForBookWithGenre1);
		}

		[Test]
		public void Get_WhenSpecifyingGenreThatDoesntExist_ShouldShowAllBooks()
		{
			var books = PopulateAndGetAllBooksExistingFromSession().ToList();

			var model = endpoint.Get(new ViewBooksLinkModel {Genre = "Genres/DoesNotExist"});

			model.ShouldContainListViewFor(books);
		}

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

		private IEnumerable<Book> PopulateAndGetAllBooksExistingFromSession()
		{
			yield return GetBookFromSessionWithId("Books/001");
			yield return GetBookFromSessionWithId("Books/002");
			yield return GetBookFromSessionWithId("Books/003");
			yield return GetBookFromSessionWithId("Books/004");
			yield return GetBookFromSessionWithId("Books/005");
			yield return GetBookFromSessionWithId("Books/006");
			yield return GetBookFromSessionWithId("Books/007");
			yield return GetBookFromSessionWithId("Books/008");
			Session.SaveChanges();
		}

		private Book GetBookFromSessionWithId(string id)
		{
			var book1 = BookTestingHelper.GetBook();
			book1.Id = id;
			Session.Store(book1);
			return book1;
		}
	}
}