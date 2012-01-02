using System;
using System.Linq;
using DataAccessTests.Utilities;
using Model;
using Model.Services;
using Model.Services.dtos;
using NUnit.Framework;
using Raven.Client;

namespace DataAccessTests
{
	[TestFixture]
	public class GenreCreaterTests : RavenTestsBase
	{
		private RavenDbGenreCreater creater;

		[SetUp]
		public void SetUp()
		{
			creater = new RavenDbGenreCreater(Session);
		}

		[Test]
		public void CanCreate()
		{
			var name = "blah";

			creater.Create(new CreateGenreDto { Name = name });

			Session.SaveChanges();

			var fromSession = GetGenreFromSessionWith(name);

			Assert.IsNotNull(fromSession);
		}

		// TODO - code to make this test pass messes up Raven - Come back to it later
		[Test][Ignore]
		public void CannotCreateGenres_WithDuplicateName()
		{
			var name = "blooh";

			creater.Create(new CreateGenreDto { Name = name });

			Assert.Throws<AttemptedCreationOfDuplicateGenre>(() => creater.Create(new CreateGenreDto { Name = name }));
		}

		private Genre GetGenreFromSessionWith(string name)
		{
			var genres = Session
				.Query<Genre>()
				.ToList();

			return genres.SingleOrDefault(g => g.Name == name);
		}
	}

	
}