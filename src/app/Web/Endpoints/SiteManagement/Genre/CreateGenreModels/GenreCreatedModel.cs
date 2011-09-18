using System;

namespace Web.Endpoints.SiteManagement.Genre.CreateGenreModels
{
	public class GenreCreatedModel
	{
		public GenreCreatedModel(string genreId)
		{
			GenreId = genreId;
		}

		public String GenreId { get; private set; }
	}
}