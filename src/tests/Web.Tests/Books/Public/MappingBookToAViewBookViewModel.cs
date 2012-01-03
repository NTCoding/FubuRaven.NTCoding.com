using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Model;
using Model.Services;
using NUnit.Framework;
using Rhino.Mocks;
using Web.Endpoints.Books;
using Web.Endpoints.Books.LinkModels;
using Web.Endpoints.Books.ViewModels;
using Web.Tests.Utilities;
using Web.Utilities;

namespace Web.Tests.Books.Public
{
	[TestFixture]
	public class ViewEndpointTests 
	{
		private ViewBookViewModel viewModel;
		private Book book;
		private IList<Book> allBooksForSameGenre;

		// TODO - perfect example for context specification - exception not the rule
		// TODO - confirms convention for testing endpoints - all the models are just nested classes that don't need to be tested
		[SetUp]
		public void WhenRequestingABookReview_AndTheSystemContainsBooks_WithDifferentGenres()
		{
			var retriever = MockRepository.GenerateMock<IBookRetriever>();
			var endpoint = new ViewEndpoint(retriever);

			book = BookTestingHelper.GetBook(rating: 4);
			
			retriever.Stub(r => r.GetById(book.Id)).Return(book);
			
			SetupBookForSameGenreIncludingReviewBook();
			retriever.Stub(r => r.GetReviewedBooks(book.Genre.Id)).Return(allBooksForSameGenre);
			
			viewModel = endpoint.Get(new ViewBookLinkModel { Id = book.Id });
		}

		private void SetupBookForSameGenreIncludingReviewBook()
		{
			allBooksForSameGenre = BookTestingHelper.GetSomeReviewedBooks(5).ToList();
			allBooksForSameGenre.Add(book);
		}

		[Test]
		public void ViewModelShouldHaveBooksTitle()
		{
			Assert.AreEqual(book.Title, viewModel.Title);
		}

		[Test]
		public void ViewModelShouldHaveBooksGenreName()
		{
			Assert.AreEqual(book.Genre.Name, viewModel.GenreName);
		}

		[Test]
		public void ViewModelShouldHaveBooksRating()
		{
			Assert.AreEqual(book.Rating, viewModel.Rating);
		}

		[Test]
		public void ViewModelShouldHaveBooksAuthors()
		{
			Assert.AreEqual(book.Authors, viewModel.Authors);
		}

		[Test]
		public void ViewModelShouldHaveBooksReview()
		{
			Assert.AreEqual(book.Review, viewModel.Review_Html);
		}

		[Test]
		public void ViewModelShouldHaveDisplayModelForBooksImage()
		{
			Assert.AreEqual(book.Id, viewModel.Image.Id);
		}

		[Test]
		public void ViewModelShouldContainRelatedBooks()
		{
			var relatedBooks = allBooksForSameGenre.Where(b => b.Id != book.Id);

			foreach (var book in relatedBooks)
			{
				var viewModelHasDisplayForBook = viewModel.RelatedBooks.Any(b => BookComparer.HasMatchingValues(b, book));

				Assert.That(viewModelHasDisplayForBook, "No book for " + book.Id);
			}
		}

		[Test]
		public void RelatedBooksShouldNotContainReviewedBook()
		{
			Assert.That(viewModel.RelatedBooks.Any(b => b.Id == book.Id), Is.False);
		}
	}
}