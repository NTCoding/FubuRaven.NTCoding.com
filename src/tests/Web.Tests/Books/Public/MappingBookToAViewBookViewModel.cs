using System.Collections.Generic;
using AutoMapper;
using Model;
using NUnit.Framework;
using Web.Endpoints.Books;
using Web.Endpoints.Books.LinkModels;
using Web.Endpoints.Books.ViewModels;
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
		// TODO - confirms convention for testing endpoints - all the models are just nested classes that don't need to be tested
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
}