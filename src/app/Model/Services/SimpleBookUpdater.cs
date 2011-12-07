using System;
using AutoMapper;
using Model.Services.dtos;
using Raven.Client;

namespace Model.Services
{
	public class SimpleBookUpdater : IBookUpdater
	{
		private readonly IDocumentSession session;

		public SimpleBookUpdater(IDocumentSession session)
		{
			this.session = session;
		}

		public void Update(UpdateBookDto dto)
		{
			var existingBook = session.Load<Book>(dto.Id);
			session.Advanced.Evict(existingBook);

			var genre = session.Load<Genre>(dto.Genre);
			var updatedBook = new Book(dto.Title, dto.Authors, dto.Description, genre, dto.Status, existingBook.Image) {Id = existingBook.Id};

			session.Store(updatedBook);
		}
	}
}