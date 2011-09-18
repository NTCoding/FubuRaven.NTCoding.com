using System;
using System.Linq;
using Model.Services;
using NUnit.Framework;
using Web.Tests;
using Web.Tests.Books;

namespace Model.Tests
{
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
}