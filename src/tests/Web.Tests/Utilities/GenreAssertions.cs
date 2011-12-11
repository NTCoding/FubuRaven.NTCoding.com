using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Web.Tests.Utilities
{
	public static class GenreAssertions
	{
		public static void ShouldMatch(this IEnumerable<Model.Genre> genres, IDictionary<string,string> mappings)
		{
			for (int i = 0; i < genres.Count(); i++)
			{
				var genre = genres.ElementAt(i);
				var mapping = mappings.ElementAt(i);
				Assert.AreEqual(genre.Id, mapping.Key);
				Assert.AreEqual(genre.Name, mapping.Value);
			}
		}
	}
}