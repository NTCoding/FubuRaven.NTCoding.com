using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model;
using Model.Services;
using Model.Services.dtos;
using NUnit.Framework;
using Rhino.Mocks;
using Web.Endpoints.SiteManagement.Book;
using Web.Endpoints.SiteManagement.Book.InputModels;
using Web.Endpoints.SiteManagement.Book.LinkModels;
using Web.Endpoints.SiteManagement.Book.ViewModels;
using Web.Tests.Utilities;
using Web.Utilities;

namespace Web.Tests.Books
{
	[TestFixture]
	public class UpdateEndpointTests : RavenTestsBase
	{
		private UpdateEndpoint endpoint;
		private IBookUpdater updater;

		[SetUp]
		public void CanCreate()
		{
			updater = MockRepository.GenerateMock<IBookUpdater>();
			endpoint = new UpdateEndpoint(Session, updater);
		}

		[Test]
		public void Get_GivenIdForBookThatLivesInSession_ShouldReturnViewModel_WithBooksDetails()
		{
			var book = GetBookThatExistsInSession();

			var result = endpoint.Get(new UpdateBookLinkModel {Id = book.Id});

			result.ShouldHaveDetailsFor(book);
		}

		[Test]
		public void Post_GivenUpdateModel_ShouldCreateDtoAndPassToBookUpdater()
		{
			var model = new UpdateBookInputModel
			            	{
			            		Authors     = new List<String> {"Jimmy", "Johnny", "Murray Walker"}.ToStringWrappers().ToList(),
			            		Genre       = "genres/9",
			            		Description_BigText = "Updated description",
			            		BookStatus      = BookStatus.Reviewed,
			            		Title       = "Updated title",
								Id          = "books/444"
							};

			endpoint.Post(model);

			UpdaterShouldHaveBeenCalledWithDtoMatching(model);
		}

		[Test]
		public void Post_ShouldReturnBookViewModel_WithIdOfBookFromModel()
		{
			var model = new UpdateBookInputModel
			            	{
			            		Id = "chewingGum"
			            	};

			var result = endpoint.Post(model);

			result.ShouldHave(model.Id);
		}

		// TODO - should pass an image to the udpater if supplied

		private void UpdaterShouldHaveBeenCalledWithDtoMatching(UpdateBookInputModel model)
		{
			updater.AssertWasCalled(x => x.Update(Arg<UpdateBookDto>.Is.Anything));

			var dto = (UpdateBookDto) (updater.GetArgumentsForCallsMadeOn(x => x.Update(Arg<UpdateBookDto>.Is.Anything))[0][0]);

			HasMatchingAuthors(model, dto);
			Assert.AreEqual(dto.Description, model.Description_BigText);
			Assert.AreEqual(dto.Genre, model.Genre);
			Assert.AreEqual(dto.Id, model.Id);
			Assert.AreEqual(dto.Status, model.BookStatus);
			Assert.AreEqual(dto.Title, model.Title);
		}

		private static void HasMatchingAuthors(UpdateBookInputModel model, UpdateBookDto dto)
		{
			// TODO -  move this logic into a list comparer - it has been implemented in the creater
			Assert.AreEqual(model.Authors.Count(), dto.Authors.Count());
			var y = dto.Authors.ToList();
			Assert.That(y.All(a => model.Authors.Any(x => a == x)), Is.True);
		}

		private Book GetBookThatExistsInSession()
		{
			var book = BookTestingHelper.GetBook();
			book.Id = "69er";
			Session.Store(book);
			Session.SaveChanges();
			return book;
		}
	}

	// TODO - move these to new classes
	public static class ViewBookLinkModelAssertions
	{
		public static void ShouldHave(this ViewBookLinkModel model, String id)
		{
			Assert.AreEqual(model.Id, id);
		}
	}

	public static class UpdateBookViewModelAssertions
	{
		public static void ShouldHaveDetailsFor(this UpdateBookViewModel model, Book book)
		{
			HasMatchingAuthors(model, book);
			Assert.AreEqual(book.Description, model.Description_BigText);
			Assert.AreEqual(book.Genre.Name, model.GenreName);
			Assert.AreEqual(book.Genre.Id, model.Genre);
			Assert.AreEqual(book.Id, model.Id);
			Assert.AreEqual(book.Status, model.BookStatus);
			Assert.AreEqual(book.Title, model.Title);
		}

		private static void HasMatchingAuthors(UpdateBookViewModel model, Book book)
		{
			Assert.AreEqual(model.Authors.Count(), book.Authors.Count());
			Assert.That(book.Authors.All(a => model.Authors.Any(x => a == x)), Is.True);
		}
	}
}