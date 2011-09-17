using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using NUnit.Framework;
using Raven.Client;

namespace Web.Tests.Books
{
	[TestFixture]
	public class CreateEndpointTests : RavenTestsBase
	{
		private CreateEndpoint _endpoint;

		[SetUp]
		public void SetUp()
		{
			base.SetUp();
			_endpoint = new CreateEndpoint(new BookCreater(Session));
		}

		[Test]
		public void Post_GivenValidBookDetails_ShouldCreateBook()
		{
			// TODO - Insert genre into session
			// validate that genre has correct name - not ID
			var model = new CreateBookInputModel
			                {
			                    Title = "Amazing Book",
			                    Genre = "1",
			                    Description = "A splendid read",
			                    Status = "Reviewed",
			                    Authors = new[] { "Jimmy Bogard", "Jimmy Slim" },
			                    Image = new[]{(byte)1}
			                };

			_endpoint.Post(model);
			Session.SaveChanges();

			var book = Session.Query<Book>()
				.Where(b => b.Title == model.Title)
				.Where(b => b.Genre.ID == model.Genre)
				.Where(b => b.Description == model.Description)
				.Where(b => b.Status == (BookStatus) Enum.Parse(typeof (BookStatus), model.Status))
				.Where(b => b.Authors.Any(a => a == model.Authors.ElementAt(0)))
				.First();

			Assert.IsNotNull(book);
			Assert.IsTrue(book.Authors.Any(a => a == model.Authors.ElementAt(1)));
		}
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

		// Validate inputs cannot be null
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

		public CreateEndpoint(IBookCreater bookCreater)
		{
			_bookCreater = bookCreater;
		}

		public void Post(CreateBookInputModel model)
		{
			_bookCreater.Create(model.Title, model.Authors, model.Description, model.Genre,
								(byte[])model.Image, model.Status);
		}
	}

	public interface IBookCreater
	{
		void Create(string title, IEnumerable<string> authors, string description, string genre, byte[] image, string status);
	}

	[TestFixture]
	public class BookCreaterTest : RavenTestsBase
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
			var title = "mega book";
			var author1 = "me";
			var author2 = "you";
			var authors = new[] { author1, author2, };
			var description = "Pretty good";
			var genreId = "1";
			var image = new[] { (byte)1 };
			var status = "Reviewed";

			_bookCreater.Create(title, authors, description, genreId, image, status);
			Session.SaveChanges();


			var book = Session.Query<Book>()
				.Where(b => b.Title == title)
				.Where(b => b.Genre.ID == genreId)
				.Where(b => b.Description == description)
				.Where(b => b.Status == (BookStatus)Enum.Parse(typeof(BookStatus), status))
				.Where(b => b.Authors.Any(a => a == author1))
				.First();

			Assert.IsNotNull(book);
		}

		// TODO - should get correct authors from session

		// TODO - should get correct genre from session

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
			var genre = new Genre("a"){ID = genreID};
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
		[ExpectedException(typeof(ArgumentNullException))]
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
			if (string.IsNullOrEmpty(name)) throw new ArgumentNullException("name");

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
