using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Web.Tests.Utilities
{
	public static class GenreAssertions
	{
		public static void ShouldMatch(this IEnumerable<Model.Genre> genres, IDictionary<string,string> mappings)
		{
			foreach (var genre in genres)
			{
				Assert.IsTrue(mappings.Any(g => g.Key == genre.Id && g.Value == genre.Name));
			}
		}
	}
}