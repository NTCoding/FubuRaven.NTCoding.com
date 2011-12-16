using System;
using System.Collections.Generic;
using AutoMapper;
using Model;
using NUnit.Framework;
using Raven.Client;
using Web.Tests.Utilities;
using Web.Utilities;

namespace Web.Tests.Books.Public
{
	[TestFixture]
	public class ViewEndpointTests : RavenTestsBase
	{
		private ViewBookViewModel model;
		private Book book;
		
		// TODO - perfect example for context specification - exception not the rule
		[SetUp]
		public void WhenRequestingABookReview()
		{
			book = BookTestingHelper.GetBook();
			book.Rating = 4;
			Session.Store(book);

			var endpoint = new ViewEndpoint(Session);
			model = endpoint.Get(new ViewBookLinkModel { Id = book.Id });

		}

		[Test]
		public void ViewModelShouldHaveBooksTitle()
		{
			Assert.AreEqual(book.Title, model.Title);
		}

		[Test]
		public void ViewModelShouldHaveBooksGenreName()
		{
			Assert.AreEqual(book.Genre.Name, model.GenreName);
		}

		// TODO - need to be able to edit rating on site management page
		[Test]
		public void ViewModelShouldHaveBooksRating()
		{
			Assert.AreEqual(book.Rating, model.Rating);
		}

		[Test]
		public void ViewModelShouldHaveBooksAuthors()
		{
			Assert.AreEqual(book.Authors, model.Authors);
		}

		[Test]
		public void ViewModelShouldHaveBooksReview()
		{
			Assert.AreEqual(book.Review, model.Review);
		}

		[Test]
		public void ViewModelShouldHaveDisplayModelForBooksImage()
		{
			Assert.AreEqual(book.Id, model.Image.Id);
		}
	}

	public class ViewBookLinkModel
	{
		public String Id { get; set; }
	}

	public class ViewEndpoint
	{
		private IDocumentSession session;

		public ViewEndpoint(IDocumentSession session)
		{
			this.session = session;
		}

		public ViewBookViewModel Get(ViewBookLinkModel linkModel)
		{
			var book = session.Load<Book>(linkModel.Id);
			
			return new ViewBookViewModel(book);
		}
	}

	public class ViewBookViewModel : Web.Endpoints.SiteManagement.Book.ViewModels.ViewBookViewModel
	{
		public ViewBookViewModel(Book book) : base(book)
		{
			
			this.Review = book.Review;
		}

		public String Review { get; set; }
	}
}