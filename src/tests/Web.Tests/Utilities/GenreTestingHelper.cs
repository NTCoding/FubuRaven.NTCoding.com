using System;
using System.Collections.Generic;

namespace Web.Tests.Utilities
{
	public static class GenreTestingHelper
	{
		// TODO - method should not rely on session
		public static Model.Genre[] Get3RandomGenres()
		{
			var g1 = new Model.Genre("good") { Id = "1" };
			var g2 = new Model.Genre("bad") { Id = "2" };
			var g3 = new Model.Genre("ugly") { Id = "3" };

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