using System;
using System.Linq;
using Model.Services;
using Model.Services.dtos;
using Model.Tests.Helpers;
using NUnit.Framework;
using Web.Tests;

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
						Title       = "mega book",
						Authors     = new[] { "me", "you", },
						Description = "Pretty good",
						Genre       = GetPersistedGenre().Id,
						Image       = new[] { (byte)1 },
						Status      = BookStatus.Reviewed
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

			var book = BookTestingHelper.GetBookFromSessionFor(dto, Session);

			Assert.IsNotNull(book);
		}

		[Test]
		public void Create_GivenValidDetails_ShouldCreateBook()
		{
			var dto = GetCreateBookDto();

			var book = _bookCreater.Create(dto);

			Assert.AreEqual(dto.Title, book.Title);
			Assert.AreEqual(dto.Description, book.Review);
			Assert.AreEqual(dto.Genre, book.Genre.Id);
			Assert.AreEqual(dto.Image, book.Image);
			Assert.AreEqual(dto.Status, book.Status);
			Assert.AreEqual(dto.Authors.Count(), book.Authors.Count());

			foreach (var author in dto.Authors)
			{
				Assert.IsTrue(book.Authors.Any(a => a == author));
			}
		}

		// TODO - validate bad inputs
	}
}