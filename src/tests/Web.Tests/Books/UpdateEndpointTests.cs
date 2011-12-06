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
using Web.Endpoints.SiteManagement.Book.LinkModels;
using Web.Endpoints.SiteManagement.Book.UpdateModels;
using Web.Endpoints.SiteManagement.Book.ViewModels;
using Web.Tests.Utilities;

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
		public void Get_ShouldTakeALinkModel_WithBooksId()
		{
			endpoint.Get(new UpdateBookLinkModel {Id = "123"});
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
			var model = new UpdateBookUpdateModel
			            	{
			            		Authors     = new List<string> {"Jimmy", "Johnny", "Murray Walker"},
			            		Genre       = "genres/9",
			            		Description = "Updated description",
			            		Status      = BookStatus.Reviewed,
			            		Title       = "Updated title",
								Id          = "books/444"
							};

			endpoint.Post(model);

			UpdaterShouldHaveBeenCalledWithDtoMatching(model);
		}

		[Test]
		public void Post_ShouldReturnBookViewModel_WithIdOfBookFromModel()
		{
			var model = new UpdateBookUpdateModel
			            	{
			            		Id = "chewingGum"
			            	};

			var result = endpoint.Post(model);

			result.ShouldHave(model.Id);
		}

		// TODO - should pass an image to the udpater if supplied

		private void UpdaterShouldHaveBeenCalledWithDtoMatching(UpdateBookUpdateModel model)
		{
			updater.AssertWasCalled(x => x.Update(Arg<UpdateBookDto>.Is.Anything));

			var dto = (UpdateBookDto) (updater.GetArgumentsForCallsMadeOn(x => x.Update(Arg<UpdateBookDto>.Is.Anything))[0][0]);

			if (HasMatchingAuthors(model, dto)
				&& dto.Description == model.Description
				&& dto.Genre  == model.Genre
				&& dto.Id     == model.Id
				&& dto.Status == model.Status
				&& dto.Title  == model.Title)
			{
				return;
			}

			throw new Exception("Properties do not match");
		}

		private static bool HasMatchingAuthors(UpdateBookUpdateModel model, UpdateBookDto dto)
		{
			// TODO -  move this logic into a list comparer
			return model.Authors.Count() == dto.Authors.Count()
				   && dto.Authors.All(model.Authors.Contains);
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
		public static void ShouldHaveDetailsFor (this UpdateBookViewModel model, Book book)
		{
			if (HasMatchingAuthors(model, book)
				&& book.Description == model.Description
				&& book.Genre.Name == model.GenreName
				&& book.Genre.Id == model.Genre
				&& book.Id == model.Id
				&& book.Status == model.Status
				&& book.Title == model.Title)
			{
				return;
			}

			throw new Exception("Properties do not match");
		}

		private static bool HasMatchingAuthors(UpdateBookViewModel model, Book book)
		{
			return model.Authors.Count() == book.Authors.Count()
			       && book.Authors.All(model.Authors.Contains);
		}
	}
}