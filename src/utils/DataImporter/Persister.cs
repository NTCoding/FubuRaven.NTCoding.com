using System;
using System.Collections.Generic;
using Model;
using Model.Services;
using Model.Services.dtos;
using Raven.Client;
using Raven.Client.Linq;
using System.Linq;

namespace DataImporter
{
	public class Persister
	{
		private readonly SqlImporter sqlImporter;
		private readonly IDocumentSession session;

		public Persister(SqlImporter sqlImporter, IDocumentSession session)
		{
			this.sqlImporter = sqlImporter;
			this.session = session;
		}

		public void Persist()
		{
			var genreDtos = sqlImporter.ImportGenres();

			//PersistGenres(genreDtos);
			
			session.SaveChanges();

			// Have to do this in 2 steps - raven no likey - comment out books, import genres, then vice versa

			PersistBooks(genreDtos);

			session.SaveChanges();
		}

		private void PersistBooks(IEnumerable<GenreDto> genreDtos)
		{
			var books = sqlImporter.ImportBooks();
			foreach (var dto in books)
			{
				var genreName = GetGenreName(genreDtos, dto);
				var genre = session.Query<Genre>().Single(g => g.Name == genreName);
				var status = (BookStatus)Enum.Parse(typeof(BookStatus), dto.Status);

				var b = new Book(dto.Title, dto.Authors, dto.Description, genre, status, dto.ImageData);
				if(dto.Rating > 0)
				{
					b.Rating = dto.Rating;
				}

				session.Store(b);
			}
		}

		private void PersistGenres(IEnumerable<GenreDto> genreDtos)
		{
			foreach (var dto in genreDtos)
			{
				session.Store(new Genre(dto.Name));
			}
		}

		private string GetGenreName(IEnumerable<GenreDto> genreDtos, BookDto dto)
		{
			var genreName = genreDtos.ToList().Single(dt => dt.Id == dto.Genre_Id).Name;

			return genreName;
		}
	}
}