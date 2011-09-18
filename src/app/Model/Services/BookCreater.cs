using System;
using System.Collections.Generic;
using System.Linq;
using Raven.Client;
using Raven.Client.Linq;

namespace Model.Services
{
	public class BookCreater : IBookCreater
	{
		private readonly IDocumentSession _session;

		public BookCreater(IDocumentSession session)
		{
			_session = session;
		}

		public void Create(string title, IEnumerable<string> authors, string description, string genreID, byte[] image, string status)
		{
			var genre = _session.Query<Genre>().Where(g => g.Id == genreID).Single();

			var bookStatus = (BookStatus) Enum.Parse(typeof (BookStatus), status);

			var book = new Book(title, authors, description, genre, bookStatus, image);

			_session.Store(book);
		}
	}
}