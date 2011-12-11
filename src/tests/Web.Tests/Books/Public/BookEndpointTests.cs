using System;
using System.Collections.Generic;
using System.Linq;
using Model;
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
			endpoint = new BookEndpoint(Session);
		}

		[Test]
		public void Get_ShouldTakeLinkModel()
		{
			endpoint.Get(new ViewBooksLinkModel());
		}
		
		[Test]
		public void Get_ShouldReturnSnapshot_ForAllExistingBooks()
		{
			var books = PopulateAndGetAllBooksExistingFromSession().ToList();

			var result = endpoint.Get(new ViewBooksLinkModel());

			result.ShouldContainListViewFor(books);
		}

		// TODO - test convention - because endpoints have specific models - the models need no tests. Covered by the endpoints

		[Test]
		public void Post_ShouldTakeInputModel()
		{
			endpoint.Post(new ViewBooksInputModel());
		}

		[Test]
		public void Post_GivenGenreId_ShouldReturnOnlyBooksWithThatGenre()
		{
			var genre1 = new Model.Genre("1");
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

			var result = endpoint.Post(new ViewBooksInputModel {Genre = genre1.Id});

			var idsForBookWithGenre1 = new[] {book1.Id, book2.Id};
			
			result.ShouldOnlyHaveBooksWith(idsForBookWithGenre1);

		}

		// TODO - move to book testing helper
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

		// TODO - should take a link model


		// Post

			// Should take a status and return books with that status
	}

	public class ViewBooksInputModel
	{
		public String Genre { get; set; }
	}

	public class ViewBooksLinkModel
	{
		
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

		public BookEndpoint(IDocumentSession session)
		{
			this.session = session;
		}

		public ViewBooksViewModel Get(ViewBooksLinkModel model)
		{
			// TODO - book retriever - could in future be replaced by a read store / view cache
			var books = session.Query<Book>()
				.ToList()
				.Select(b => new BookListView(b));

			// TODO - map this to a view-specific status so we don't display all statuses = e.g hidden
			return new ViewBooksViewModel(books);
		}

		public ViewBooksViewModel Post(ViewBooksInputModel model)
		{
			// TODO - book retriever
			var books = session.Query<Book>()
				.Where(b => b.Genre.Id == model.Genre)
				.ToList()
				.Select(b => new BookListView(b));

			return new ViewBooksViewModel(books);
		}
	}

	public class ViewBooksViewModel
	{
		public ViewBooksViewModel(IEnumerable<BookListView> books)
		{
			Books = books;
		}

		public IEnumerable<BookListView> Books { get; set; }
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