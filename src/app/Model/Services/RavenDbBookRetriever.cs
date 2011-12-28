using System;
using System.Collections.Generic;
using System.Linq;
using Raven.Client;

namespace Model.Services
{
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

		public Book GetById(string id)
		{
			return session.Load<Book>(id);
		}
	}
}