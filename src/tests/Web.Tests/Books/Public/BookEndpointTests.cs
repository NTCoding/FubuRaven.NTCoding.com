using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using Model.Services;
using NUnit.Framework;
using Raven.Client;
using Web.Tests.Utilities;

namespace Web.Tests.Books.Public
{
	// TODO - move the raven tests base into a common testing project
	//        and remove the dependency between testing projects
	[TestFixture]
	public class BookEndpointTests : RavenTestsBase
	{
		private BookEndpoint endpoint;

		[SetUp]
		public void CanCreate()
		{
			// TODO - so this is an integration test - how are we defining when to verify calls and when to use components?
			//        I think using a component and not mocking the interface still verifies the behaviour of this component
			//        it tells us that this unit works correctly with an implementation of the interface that also works
			endpoint = new BookEndpoint(Session, new RavenDbGenreRetriever(Session));
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

	public class ViewBooksLinkModel
	{
		public String Genre { get; set; }
	}

	public class BookListView
	{
		public BookListView(Book book)
		{
			this.Id = book.Id;
			this.Image = book.Image;
			this.Title = book.Title;
		}

		public String Id { get; set; }

		public byte[] Image { get; set; }

		public String Title { get; set; }
	}

	public class BookEndpoint
	{
		private readonly IDocumentSession session;
		private readonly IGenreRetriever genreRetriever;

		public BookEndpoint(IDocumentSession session, IGenreRetriever genreRetriever)
		{
			this.session = session;
			this.genreRetriever = genreRetriever;
		}

		public ViewBooksViewModel Get(ViewBooksLinkModel model)
		{
			// TODO - book retriever - could in future be replaced by a read store / view cache
			var models = GetBooks(model).ToList().Select(b => new BookListView(b));

			return new ViewBooksViewModel(models, genreRetriever.GetAllOrderedByName());
		}

		private IQueryable<Book> GetBooks(ViewBooksLinkModel model)
		{
			return ShouldDefaultToAllGenres(model) 
				? session.Query<Book>()
				: session.Query<Book>().Where(b => b.Genre.Id == model.Genre);
		}

		private bool ShouldDefaultToAllGenres(ViewBooksLinkModel model)
		{
			return string.IsNullOrWhiteSpace(model.Genre);
		}
	}

	public class ViewBooksViewModel
	{
		public ViewBooksViewModel(IEnumerable<BookListView> books, IDictionary<string, string> genres)
		{
			Books = books;
			Genres = genres;
		}

		public IEnumerable<BookListView> Books { get; set; }

		public IDictionary<String, String> Genres { get; set; }
	}

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
			       && bookListView.Image == book.Image
			       && bookListView.Title == book.Title;
		}

		public static void ShouldOnlyHaveBooksWith(this ViewBooksViewModel model, IEnumerable<String> ids)
		{
			Assert.AreEqual(ids.Count(), model.Books.Count());

			foreach (var id in ids)
			{
				Assert.That(model.Books.Any(b => b.Id == id), "No id for: " + id);
			}
		}
	}
}