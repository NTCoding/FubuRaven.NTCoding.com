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
			var genre = GetGenreThatExistsInSession();

			var dto = new UpdateBookDto
			          	{
			          		Id          = book.Id,
			          		Authors     = new[] {"hey", "hooo", "haaaa"},
			          		Description = "Update description - dfkjldkj fldjflkd ",
			          		Genre       = genre.Id,
			          		Status      = (BookStatus) ((int) book.Status) + 1,
			          		Title       = "Update title"
			          	};

			updater.Update(dto);

			Session.SaveChanges();

			SessionShouldContainBookWithUpdatedValuesFrom(dto);
		}

		private Book GetBookThatExistsInSession()
		{
			var genre = new Genre("a");

			var book = new Book("blah", new[] {"me", "you", "him"}, "descy", 
				genre, BookStatus.CurrentlyReading,new[] {(byte) 1});
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

		// TODO - if the dto has a null image - do not update

		// TODO - if the image is not null - should update it
	}
}