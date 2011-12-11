using Raven.Client;

namespace Web.Tests.Utilities
{
	public static class GenreTestingHelper
	{
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
	}
}