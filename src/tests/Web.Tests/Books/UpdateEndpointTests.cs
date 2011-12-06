using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Model;
using Model.Services;
using NUnit.Framework;
using Raven.Client;
using Rhino.Mocks;
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

		// POST
			// TODO - validation / failure scenario
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

		// TODO - Should redirect to the view book page - return type?

		private Book GetBookThatExistsInSession()
		{
			var book = BookTestingHelper.GetBook();
			book.Id = "69er";
			Session.Store(book);
			Session.SaveChanges();
			return book;
		}

		// TODO - what happens wht ID is null?

		
	}

	public class UpdateBookDto : CreateBookDto
	{
		public UpdateBookDto(UpdateBookUpdateModel model)
		{
			Mapper.DynamicMap(model, this);
		}

		public string Id { get; set; }
	}

	public interface IBookUpdater
	{
		void Update(UpdateBookDto dto);
	}

	public class UpdateBookLinkModel
	{
		public String Id { get; set; }
	}

	public class UpdateEndpoint
	{
		private readonly IDocumentSession session;
		private readonly IBookUpdater updater;

		public UpdateEndpoint(IDocumentSession session, IBookUpdater updater)
		{
			this.session = session;
			this.updater = updater;
		}

		public UpdateBookViewModel Get(UpdateBookLinkModel model)
		{
			// TODO - should controllers even see the domain entities?
			var book = session.Load<Book>(model.Id);

			return new UpdateBookViewModel(book);
		}

		public void Post(UpdateBookUpdateModel model)
		{
			updater.Update(new UpdateBookDto(model));
		}
	}

	public class UpdateBookUpdateModel
	{
		public IList<String> Authors { get; set; }
		
		public String Description { get; set; }
		
		public BookStatus Status { get; set; }
		
		public String Genre { get; set; }
		
		public String Title { get; set; }

		public string Id { get; set; }
	}

	public class UpdateBookViewModel : UpdateBookUpdateModel
	{
		public UpdateBookViewModel(Book book)
		{
			Mapper.DynamicMap(book, this);
			if (book != null) this.Genre = book.Genre.Id;
		}

		public String Id { get; set; }

		public String GenreName { get; private set; }
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