using System;
using System.Linq;
using Raven.Client;
using Web.Endpoints.SiteManagement.Genre.CreateGenreModels;

namespace Web.Endpoints.SiteManagement.Genre
{
	public class CreateEndpoint
	{
		private readonly IDocumentSession _session;

		public CreateEndpoint(IDocumentSession session)
		{
			_session = session;
		}

		public String Post(CreateGenreInputModel model)
		{
			// TODO - offload to a service
			var genre = new Model.Genre(model.Name);
			_session.Store(genre);

			return genre.Id;
		}
	}
}