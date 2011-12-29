using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using Model.Services.dtos;
using Raven.Client;

namespace DataAccessTests.Utilities
{
	public class BookTestingHelper
	{
		public static Book GetBookFromSessionFor(CreateBookDto dto, IDocumentSession session)
		{
			// TODO - turn this into a RavenDB index
			return session.Query<Book>()
				.Where(b => b.Title == dto.Title)
				.Where(b => b.Genre.Id == dto.Genre)
				.Where(b => b.Review == dto.Description)
				.Where(b => b.Status == dto.Status)
				.Where(b => b.Authors.Any(a => a == dto.Authors.First()))
				.FirstOrDefault();
		}

		public static IEnumerable<Book> CreateBooks(int number, BookStatus status, Genre genre = null)
		{
			for (int i = 0; i < number; i++)
			{
				var book = new Book("blah", new[] {"jim", "John", "Jackie"}, "crap", genre ??new Genre("gumbo"), status, new[] {(byte) 1});
				yield return book;
			}
		}
	}
}