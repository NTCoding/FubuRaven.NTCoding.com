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

		private Genre GetPersistedGenre()
		{
			var genre = new Genre("Jimmy Bogard") { Id = "99" };
			Session.Store(genre);
			Session.SaveChanges();
			return genre;
		}

		private CreateBookDto GetCreateBookDto()
		{
			return new CreateBookDto
			       	{
						Title = "mega book",
						Authors = new[] { "me", "you", },
						Description = "Pretty good",
						Genre = GetPersistedGenre().Id,
						Image = new[] { (byte)1 },
						Status = BookStatus.Reviewed
			       	};
		}

		[SetUp]
		public void SetUp()
		{
			_bookCreater = new BookCreater(Session);
		}

		[Test]
		public void Create_GivenValidBookDetails_BookShouldBePersisted()
		{
			var dto = GetCreateBookDto();

			_bookCreater.Create(dto);
			Session.SaveChanges();

			var book = Session.Query<Book>()
				.Where(b => b.Title == dto.Title)
				.Where(b => b.Genre.Id == dto.Genre)
				.Where(b => b.Description == dto.Description)
				.Where(b => b.Status == dto.Status)
				.Where(b => b.Authors.Any(a => a == dto.Authors.First()))
				.First();

			Assert.IsNotNull(book);
		}

		[Test]
		public void Create_GivenValidDetails_ShouldCreateBook()
		{
			var dto = GetCreateBookDto();

			var book = _bookCreater.Create(dto);
		}

		// TODO - validate bad inputs
	}
}