﻿using System;
using System.Linq;
using Model.Services.dtos;
using Raven.Client;

namespace Model.Services
{
	public class BookCreater : IBookCreater
	{
		private readonly IDocumentSession _session;

		public BookCreater(IDocumentSession session)
		{
			_session = session;
		}

		// TODO - Should only return the Id
		public String Create(CreateBookDto createBookDto)
		{
			var genre = _session.Query<Genre>().Where(g => g.Id == createBookDto.Genre).Single();

			var book = new Book(createBookDto.Title, createBookDto.Authors, createBookDto.Description, genre, createBookDto.Status, createBookDto.Image);

			_session.Store(book);

			return book.Id;
		}
	}
}