using System;
using System.Collections.Generic;
using Raven.Client;

namespace Web.Tests.Utilities
{
	public static class GenreTestingHelper
	{
		// TODO - method should not rely on session
		public static Model.Genre[] GetGenresFromSession(IDocumentSession session)
		{
			var g1 = new Model.Genre("good") { Id = "1" };
			var g2 = new Model.Genre("bad") { Id = "2" };
			var g3 = new Model.Genre("ugly") { Id = "3" };

			session.Store(g1);
			session.Store(g2);
			session.Store(g3);
			session.SaveChanges();

			return new[] { g1, g2, g3 };
		}

		public static IEnumerable<Model.Genre> GetGenresWithDifferentNames()
		{
			yield return new Model.Genre("good") { Id = "1" };
			yield return new Model.Genre("bad") { Id = "2" };
			yield return new Model.Genre("ugly") { Id = "3" };
		}
	}
}