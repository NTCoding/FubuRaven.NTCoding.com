using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using Model.Services;
using Raven.Client;
using Web.Endpoints.Books.LinkModels;
using Web.Endpoints.Books.ViewModels;

namespace Web.Endpoints.Books
{
	public class IndexEndpoint
	{
		// TODO - Testing convention - controllers talk to services but not the document session
		//                             tests verify the services were called
		//                             the services get tested with the in memory database
		private readonly IGenreRetriever genreRetriever;
		private readonly IBookRetriever bookRetriever;

		public IndexEndpoint(IGenreRetriever genreRetriever, IBookRetriever bookRetriever)
		{
			this.bookRetriever = bookRetriever;
			this.genreRetriever = genreRetriever;
		}

		public ViewBooksViewModel Get(ViewBooksLinkModel model)
		{
			var models = GetBooks(model).ToList().Select(b => new BookListView(b));
			var wishlistBooks = bookRetriever.GetWishlistBooks().ToList().Select(b => new BookListView(b));
			
			return new ViewBooksViewModel(models, genreRetriever.GetAllOrderedByName(), model.Genre, wishlistBooks);
		}
		

		private IEnumerable<Book> GetBooks(ViewBooksLinkModel model)
		{
			var shouldDefaultToAllGenres = ShouldDefaultToAllGenres(model);

			return shouldDefaultToAllGenres 
			       	? bookRetriever.GetReviewedBooksOrderedByRating()
			       	: bookRetriever.GetReviewedBooksOrderedByRating(model.Genre);
		}

		private bool ShouldDefaultToAllGenres(ViewBooksLinkModel model)
		{
			return string.IsNullOrWhiteSpace(model.Genre)
			       || !genreRetriever.CanFindGenreWith(model.Genre);
		}
	}

	public class RavenDbBookRetriever : IBookRetriever
	{
		private readonly IDocumentSession session;

		public RavenDbBookRetriever(IDocumentSession session)
		{
			this.session = session;
		}

		public IEnumerable<Book> GetReviewedBooksOrderedByRating(String genre = null)
		{
			IEnumerable<Book> books = session.Query<Book>()
				.Where(b => b.Status == BookStatus.Reviewed)
				.OrderByDescending(b => b.Rating);

			if (genre != null) books = books.Where(b => b.Genre.Id == genre);


			return books;
		}

		public IEnumerable<Book> GetWishlistBooks()
		{
			return session.Query<Book>().Where(b => b.Status == BookStatus.Wishlist);
		}
	}
}