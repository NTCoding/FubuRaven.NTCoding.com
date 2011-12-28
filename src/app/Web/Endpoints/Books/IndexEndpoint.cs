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
		private readonly IGenreRetriever genreRetriever;
		private readonly IBookRetriever bookRetriever;

		public IndexEndpoint(IGenreRetriever genreRetriever, IBookRetriever bookRetriever)
		{
			this.bookRetriever = bookRetriever;
			this.genreRetriever = genreRetriever;
		}

		public ViewBooksViewModel Get(ViewBooksLinkModel model)
		{
			var books = GetBooks(model);
			var models = books.ToList().Select(b => new BookListView(b));
			var wishlistBooks = bookRetriever.GetWishlistBooks().ToList().Select(b => new BookListView(b));
			
			return new ViewBooksViewModel(models, genreRetriever.GetAll(), model.Genre, wishlistBooks);
		}
		

		private IEnumerable<Book> GetBooks(ViewBooksLinkModel model)
		{
			var shouldDefaultToAllGenres = ShouldDefaultToAllGenres(model);

			return shouldDefaultToAllGenres 
			       	? bookRetriever.GetReviewedBooks()
			       	: bookRetriever.GetReviewedBooks(model.Genre);
		}

		// TODO - Should this logic live here? 100% yes - it is application behaviour specific to this view
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

		public IEnumerable<Book> GetReviewedBooks(String genre = null)
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