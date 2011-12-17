using System;
using System.Collections.Generic;
using System.Linq;
using Raven.Client;

namespace Model.Services
{
	public class RavenDbGenreRetriever : IGenreRetriever
	{
		private readonly IDocumentSession session;

		public RavenDbGenreRetriever(IDocumentSession session)
		{
			this.session = session;
		}

		public IDictionary<string, string> GetAllOrderedByName()
		{
			return session
				.Query<Model.Genre>()
				.OrderBy(g => g.Name)
				.ToDictionary(g => g.Id, g => g.Name);
		}

		public bool CanFindGenreWith(string id)
		{
			return session.Query<Genre>().Count(g => g.Id == id) > 0;
		}
	}
}