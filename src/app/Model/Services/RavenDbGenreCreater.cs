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
			var genre = new Genre(dto.Name);

			session.Store(genre);

			return genre.Id;
		}
	}
}