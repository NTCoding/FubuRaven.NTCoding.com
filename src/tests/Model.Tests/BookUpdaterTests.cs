using System;
using Model.Services;
using Model.Services.dtos;
using Model.Tests.Helpers;
using NUnit.Framework;
using Web.Tests;

namespace Model.Tests
{
	[TestFixture]
	public class BookUpdaterTests : RavenTestsBase
	{
		private SimpleBookUpdater updater;

		[SetUp]
		public void SetUp()
		{
			updater = new SimpleBookUpdater(Session);
		}

		[Test]
		public void Update_GivenDtoForBookInSession_ShouldUpdateBookInSession()
		{
			var book = GetBookThatExistsInSession();

			var dto = new UpdateBookDto
			          	{
			          		Id          = book.Id,
							Genre       = book.Genre.Id,
			          		Authors     = new[] {"hey", "hooo", "haaaa"},
			          		Description = "Update description - dfkjldkj fldjflkd ",
			          		Status      = (BookStatus) ((int) book.Status) + 1,
			          		Title       = "Update title"
			          	};

			updater.Update(dto);

			Session.SaveChanges();

			SessionShouldContainBookWithUpdatedValuesFrom(dto);
		}

		[Test]
		public void Update_WhenNoImageSupplied_ShouldKeepExistingOne()
		{
			var book = GetBookThatExistsInSession();

			var dto = new UpdateBookDto {Id = book.Id, Genre = book.Genre.Id};

			updater.Update(dto);

			Session.SaveChanges();

			SessionShouldBookWithImage(book.Id, book.Image);
		}

		[Test]
		public void Update_WhenImageSupplied_ShouldReplaceExistingOne()
		{
			var book = GetBookThatExistsInSession();

			var newImage = new[] { (byte)88, (byte)11, (byte)0 };
			var dto = new UpdateBookDto {Id = book.Id, Genre = book.Genre.Id, Image = newImage};

			updater.Update(dto);

			Session.SaveChanges();

			SessionShouldBookWithImage(book.Id, newImage);
		}

		private Book GetBookThatExistsInSession(byte[] image = null)
		{
			var genre = new Genre("a");

			var book = new Book("blah", new[] {"me", "you", "him"}, "descy",
				GetGenreThatExistsInSession(), BookStatus.CurrentlyReading, image ?? new[] { (byte)1 });
			book.Id = "books/999";

			Session.Store(genre);
			Session.Store(book);

			Session.SaveChanges();

			return book;
		}

		private Genre GetGenreThatExistsInSession()
		{
			var genre = new Genre("xyz");
			Session.Store(genre);
			Session.SaveChanges();

			return genre;
		}

		private void SessionShouldContainBookWithUpdatedValuesFrom(UpdateBookDto dto)
		{
			var updatedBook = Session.Load<Book>(dto.Id);

			Assert.AreEqual(dto.Authors, updatedBook.Authors);
			Assert.AreEqual(dto.Description, updatedBook.Description);
			Assert.AreEqual(dto.Genre, updatedBook.Genre.Id);
			Assert.AreEqual(dto.Status, updatedBook.Status);
			Assert.AreEqual(dto.Title, updatedBook.Title);

			Assert.IsNotNull(updatedBook);
		}

		private void SessionShouldBookWithImage(string id, byte[] image)
		{
			var book = Session.Load<Book>(id);

			Assert.AreEqual(image, book.Image);
		}
	}
}