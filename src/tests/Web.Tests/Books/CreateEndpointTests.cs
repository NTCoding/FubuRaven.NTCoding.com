using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using FubuMVC.Core.Continuations;
using NUnit.Framework;
using Raven.Client;

namespace Web.Tests.Books
{
	[TestFixture]
	public class CreateEndpointTests : RavenTestsBase
	{
		private CreateBookInputModel GetTestCreateBookInputModel(Genre genre)
		{
			return new CreateBookInputModel
			{
				Title = "Amazing Book",
				Genre = genre.ID,
				Description = "A splendid read",
				Status = "Reviewed",
				Authors = new[] { "Jimmy Bogard", "Jimmy Slim" },
				Image = new[] { (byte)1 }
			};
		}

		private Genre GetGenreFromSession()
		{
			var genre = new Genre("wooo") { ID = "1" };
			Session.Store(genre);
			Session.SaveChanges();
			return genre;
		}

		private CreateEndpoint _endpoint;

		[SetUp]
		public void SetUp()
		{
			base.SetUp();
			_endpoint = new CreateEndpoint(new BookCreater(Session), Session);
		}

		[Test]
		public void Get_ShouldBeAccessbileFromCreateBookLinkModel()
		{
			_endpoint.Get(new CreateBookLinkModel());
		}

		[Test]
		public void Get_ViewModelShouldContainAllGenres()
		{
			var g1 = new Genre("good") {ID = "1"};
			var g2 = new Genre("bad") {ID = "2"};
			var g3 = new Genre("ugly") {ID = "3"};

			Session.Store(g1);
			Session.Store(g2);
			Session.Store(g3);
			Session.SaveChanges();

			var viewModel = _endpoint.Get(new CreateBookLinkModel());

			Assert.IsTrue(viewModel.Genres.Any(g => g.Key == g1.ID && g.Value == g1.Name));
			Assert.IsTrue(viewModel.Genres.Any(g => g.Key == g2.ID && g.Value == g2.Name));
			Assert.IsTrue(viewModel.Genres.Any(g => g.Key == g3.ID && g.Value == g3.Name));
		}

		[Test]
		public void Post_GivenValidBookDetails_ShouldCreateBook()
		{
			var genre = GetGenreFromSession();

			var model = GetTestCreateBookInputModel(genre);

			_endpoint.Post(model);
			Session.SaveChanges();

			var book = Session.Query<Book>()
				.Where(b => b.Title == model.Title)
				.Where(b => b.Genre.Name == genre.Name)
				.Where(b => b.Description == model.Description)
				.Where(b => b.Status == (BookStatus) Enum.Parse(typeof (BookStatus), model.Status))
				.Where(b => b.Authors.Any(a => a == model.Authors.ElementAt(0)))
				.First();

			Assert.IsNotNull(book);
			Assert.IsTrue(book.Authors.Any(a => a == model.Authors.ElementAt(1)));
		}


		[Test]
		public void Post_ShouldRedirectToMangementViewBook()
		{
			var genre = GetGenreFromSession();
			var model = GetTestCreateBookInputModel(genre);

			var result = _endpoint.Post(model);

			result.AssertWasRedirectedTo<ViewEndpoint>(e => e.Get(new ViewBookLinkModel()));
		}
	}

	[TestFixture]
	public class ViewEndpointTests
	{
		private ViewEndpoint _endpoint;

		[SetUp]
		public void CanCreate()
		{
			_endpoint = new ViewEndpoint();
		}

		[Test]
		public void Get_ShouldBeLinkedToFromViewBookLinkModel()
		{
			_endpoint.Get(new ViewBookLinkModel());
		}
	}

	[TestFixture]
	public class ViewBookLinkModelTest
	{
		[Test]
		public void CanCreate()
		{
			new ViewBookLinkModel();
		}
	}

	public class ViewBookLinkModel
	{
	}

	public class ViewEndpoint
	{
		public object Get(ViewBookLinkModel model)
		{
			return null;
		}
	}

	[TestFixture]
	public class CreateBookViewModelTests
	{
		[Test]
		public void CanCreate()
		{
			var genres = new Dictionary<String, String>();
			genres.Add("1", "genre1");

			new CreateBookViewModel(genres);
		}

		[Test]
		public void ShouldConstructGenres()
		{
			var genres = new Dictionary<String, String>();
			var genreID = "1";
			var genreName = "genre1";

			genres.Add(genreID, genreName);

			var model = new CreateBookViewModel(genres);

			Assert.IsTrue(model.Genres.Single().Key == genreID && model.Genres.Single().Value == genreName);
		}
	}

	public class CreateBookViewModel
	{
		public CreateBookViewModel(Dictionary<string, string> genres)
		{
			Genres = genres;
		}

		public IDictionary<String, String> Genres { get; private set; }
	}

	[TestFixture]
	public class CreateBookLinkModelTests
	{
		[Test]
		public void CanCreate()
		{
			new CreateBookLinkModel();
		}
	}

	public class CreateBookLinkModel
	{
	}

	[TestFixture]
	public class BookTests
	{
		private Book GetValidBook()
		{
			string title = "hello mommy";
			var authors = new[] { "jim bowen" };
			var description = "not very good";
			var genre = new Genre("Books");
			var status = BookStatus.Reviewed;
			var image = new[] { (byte)1 };

			return new Book(title, authors, description, genre, status, image);
		}

		[Test]
		public void CanCreate()
		{
			GetValidBook();
		}

		[Test]
		public void ShouldConstruct()
		{
			string title = "hello mommy";
			var authors = new[] { "jim bowen" };
			var description = "not very good";
			var genre = new Genre("Books");
			var status = BookStatus.Reviewed;
			var image = new[] { (byte)1 };

			var book = new Book(title, authors, description, genre, status, image);

			Assert.AreEqual(title, book.Title);
			Assert.AreEqual(authors, book.Authors);
			Assert.AreEqual(description, book.Description);
			Assert.AreEqual(genre, book.Genre);
			Assert.AreEqual(status, book.Status);
			Assert.AreEqual(image, book.Image);
		}

		[Test]
		public void ShouldHaveAnIDProperty()
		{
			var b = GetValidBook();

			var x = b.ID;
		}

		// TODO Validate inputs cannot be null
	}

	public class Book
	{
		public Book(string title, IEnumerable<String> authors, string description, Genre genre, BookStatus status, byte[] image)
		{
			Title       = title;
			Authors     = authors;
			Description = description;
			Genre       = genre;
			Status      = status;
			Image       = image;
		}

		public String Title { get; private set; }

		public String ID { get; set; }

		public Genre Genre { get; private set; }

		public String Description { get; private set; }

		public BookStatus Status { get; private set; }

		public IEnumerable<String> Authors { get; private set; }

		public byte[] Image { get; private set; }
	}

	public class CreateEndpoint
	{
		private readonly IBookCreater _bookCreater;
		private readonly IDocumentSession _session;

		public CreateEndpoint(IBookCreater bookCreater, IDocumentSession session)
		{
			_bookCreater = bookCreater;
			_session = session;
		}

		public FubuContinuation Post(CreateBookInputModel model)
		{
			_bookCreater.Create(model.Title, model.Authors, model.Description, model.Genre,
								(byte[])model.Image, model.Status);

			return FubuContinuation.RedirectTo<ViewEndpoint>(e => e.Get(new ViewBookLinkModel()));
		}

		public CreateBookViewModel Get(CreateBookLinkModel model)
		{
			var genres = _session
				.Query<Genre>()
				.ToDictionary(g => g.ID, g => g.Name);

			return new CreateBookViewModel(genres);
		}
	}

	public interface IBookCreater
	{
		void Create(string title, IEnumerable<string> authors, string description, string genre, byte[] image, string status);
	}

	[TestFixture]
	public class BookCreaterTests : RavenTestsBase
	{
		private BookCreater _bookCreater;

		[SetUp]
		public void SetUp()
		{
			_bookCreater = new BookCreater(Session);
		}

		[Test]
		public void Create_GivenValidBookDetails_ShouldCreateBook()
		{
			var genre = new Genre("Jimmy Bogard") {ID = "99"};
			Session.Store(genre);
			Session.SaveChanges();

			var title = "mega book";
			var author1 = "me";
			var author2 = "you";
			var authors = new[] { author1, author2, };
			var description = "Pretty good";
			var genreId = genre.ID;
			var image = new[] { (byte)1 };
			var status = "Reviewed";

			_bookCreater.Create(title, authors, description, genreId, image, status);
			Session.SaveChanges();


			var book = Session.Query<Book>()
				.Where(b => b.Title == title)
				.Where(b => b.Genre.Name == genre.Name)
				.Where(b => b.Description == description)
				.Where(b => b.Status == (BookStatus)Enum.Parse(typeof(BookStatus), status))
				.Where(b => b.Authors.Any(a => a == author1))
				.First();

			Assert.IsNotNull(book);
		}

		// TODO - validate bad inputs
	}

	public class BookCreater : IBookCreater
	{
		private readonly IDocumentSession _session;

		public BookCreater(IDocumentSession session)
		{
			_session = session;
		}

		public void Create(string title, IEnumerable<string> authors, string description, string genreID, byte[] image, string status)
		{
			var genre = _session.Query<Genre>().Where(g => g.ID == genreID).Single();

			var bookStatus = (BookStatus) Enum.Parse(typeof (BookStatus), status);

			var book = new Book(title, authors, description, genre, bookStatus, image);

			_session.Store(book);
		}
	}

	[TestFixture]
	public class CreateBookInputModelTests
	{
		[Test]
		public void CanCreate()
		{
			new CreateBookInputModel();
		}
	}

	public class CreateBookInputModel
	{
		public string Title { get; set; }

		public String Genre { get; set; }

		public string Description { get; set; }

		public string Status { get; set; }

		public IEnumerable<String> Authors { get; set; }

		// TODO - test a file upload?
		public object Image { get; set; }
	}

	[TestFixture]
	public class GenreTest
	{
		[Test]
		public void CanCreate()
		{
			new Genre("Cheese");
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void IfEmptyStringSupplied_ShouldThrowException()
		{
			new Genre("");
		}

		[Test]
		public void WhenCreating_ShouldSetName()
		{
			var name = "MyNameIsGenre";
			
			var genre = new Genre(name);

			Assert.AreEqual(name, genre.Name);
		}

		[Test]
		public void ShouldHaveAnID()
		{
			var genre = new Genre("abc");

			var x = genre.ID;
		}
	}

	public class Genre
	{
		public Genre(string name)
		{
			if (string.IsNullOrEmpty(name)) throw new ArgumentException("name");

			Name = name;
		}

		public String Name { get; private set; }

		public String ID { get; set; }
	}

	public enum BookStatus
	{
		Wishlist,
		CurrentlyReading,
		Reviewed
	}
}
