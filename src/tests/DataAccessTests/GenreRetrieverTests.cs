using System;
using System.Collections.Generic;
using System.Linq;
using DataAccessTests.Utilities;
using Model;
using Model.Services;
using NUnit.Framework;

namespace DataAccessTests
{
	[TestFixture]
	public class GenreRetrieverTests : RavenTestsBase
	{
		private RavenDbGenreRetriever retriever;

		[SetUp]
		public void SetUp()
		{
			retriever = new RavenDbGenreRetriever(Session);
		}

		[Test]
		public void GetAll_ShouldReturnAllGenres_ExistingInSession()
		{
			var genres = CreateSomeGenres(10);

			genres.ToList().ForEach(Session.Store);

			Session.SaveChanges();

			var fromRetriever = retriever.GetAll();
			var genreNames = fromRetriever.Select(r => r.Value);

			ShouldMatch(genreNames, genres);
		}

		private void ShouldMatch(IEnumerable<string> genreNames, IEnumerable<Genre> genres)
		{
			foreach (var name in genreNames)
			{
				Assert.IsTrue(genres.Any(g => g.Name == name));
			}
		}

		private IEnumerable<Genre> CreateSomeGenres(int amount)
		{
			for (int i = 0; i < amount; i++)
			{
				yield return new Genre("Genre " + i);
			}

		}

		[Test]
		public void FindById_ShouldReturnGenre_WithGivenId()
		{
			Assert.Inconclusive();
		}
	}
}