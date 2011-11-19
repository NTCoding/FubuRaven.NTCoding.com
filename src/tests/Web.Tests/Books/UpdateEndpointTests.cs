using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using NUnit.Framework;
using Raven.Client;
using Web.Tests.Utilities;

namespace Web.Tests.Books
{
	[TestFixture]
	public class UpdateEndpointTests : RavenTestsBase
	{
		private UpdateEndpoint endpoint;
		// GET 
			// should take a link model with the books id
			
			// should return a book view model with book's details

		[SetUp]
		public void CanCreate()
		{
			endpoint = new UpdateEndpoint(Session);
		}

		[Test]
		public void Get_ShouldTakeALinkModel_WithBooksId()
		{
			endpoint.Get(new UpdateBookLinkModel {Id = "123"});
		}

		[Test][Ignore] 
		public void Get_GivenIdForBookThatLivesInSession_ShouldReturnViewModel_WithBooksDetails()
		{
			// get a book that is added to the session
			var book = BookTestingHelper.GetBook();
			book.Id = "69er";
			Session.Store(book);
			Session.SaveChanges();

			// use book's id in the link model
			var result = endpoint.Get(new UpdateBookLinkModel {Id = book.Id});

			// verify the session was queried for the book's id
			result.ShouldHaveDetailsFor(book);
		}

		// TODO - should return a book view model

		// TODO - should blow up if ID Null

		// POST
			// TODO - validation / failure scenario

			// Should create dto and pass to the book updater

			// Should redirect to the view book page
	}

	public class UpdateBookLinkModel
	{
		public String Id { get; set; }
	}

	public class UpdateEndpoint
	{
		private readonly IDocumentSession session;

		public UpdateEndpoint(IDocumentSession session)
		{
			this.session = session; 
		}

		public UpdateBookViewModel Get(UpdateBookLinkModel model)
		{
			// TODO - should controllers even see the domain entities?
			var book = session.Load<Book>(model.Id);

			return new UpdateBookViewModel(book);
		}
	}

	public class UpdateBookViewModel
	{
		public UpdateBookViewModel(Book book)
		{
		}

		public IList<String> Authors { get; set; }

		public String Description { get; set; }

		public String Id { get; set; }

		public BookStatus Status { get; set; }

		public String GenreName { get; set; }

		public String GenreId { get; set; }

		public String Title { get; set; }
	}

	public static class UpdateBookViewModelAssertions
	{
		public static void ShouldHaveDetailsFor (this UpdateBookViewModel model, Book book)
		{
			if (HasMatchingAuthors(model, book)
				&& book.Description == model.Description
				&& book.Genre.Name == model.GenreName
				&& book.Genre.Id == model.GenreId
				&& book.Id == model.Id
				&& book.Status == model.Status
				&& book.Title == model.Title)
			{
				return;
			}
		}

		private static bool HasMatchingAuthors(UpdateBookViewModel model, Book book)
		{
			return model.Authors.Count() == book.Authors.Count()
			       && book.Authors.All(model.Authors.Contains);
		}
	}
}