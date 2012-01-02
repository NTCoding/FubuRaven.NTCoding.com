using System;
using System.Linq;
using Model.Services.dtos;
using Raven.Client;

namespace Model.Services
{
	public class RavenDbGenreCreater : IGenreCreater
	{
		private readonly IDocumentSession session;

		public RavenDbGenreCreater(IDocumentSession session)
		{
			this.session = session;
		}

		public string Create(CreateGenreDto dto)
		{
			//if (IsAlreadyGenreWith(dto.Name)) throw new AttemptedCreationOfDuplicateGenre();

			var genre = new Genre(dto.Name);

			session.Store(genre);

			return genre.Id;
		}

		private bool IsAlreadyGenreWith(string name)
		{
			return session
				.Query<Genre>()
				.Any(g => g.Name == name);
		}
	}

	public class AttemptedCreationOfDuplicateGenre : Exception
	{
	}
}