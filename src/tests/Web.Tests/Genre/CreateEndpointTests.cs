using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Raven.Client;

namespace Web.Tests.Genre
{
	[TestFixture]
	public class CreateEndpointTests : RavenTestsBase
	{
		private CreateEndpoint _endpoint;

		[SetUp]
		public void CanCreate()
		{
			_endpoint = new CreateEndpoint(Session);
		}

		[Test]
		public void Post_GivenGenreName_ShouldCreateGenre()
		{
			string name = "Wickedd!!!";
			_endpoint.Post(new CreateGenreInputModel {Name = name});
			Session.SaveChanges();

			Assert.IsNotNull(Session.Query<Model.Genre>().Single(g => g.Name == name));
		}

		[Test]
		public void Post_GivenGenreName_ResultShouldContainNewGenresID()
		{
			string name = "moomin";
			var result =_endpoint.Post(new CreateGenreInputModel {Name = name});
			
			Session.SaveChanges();

			var createdGenre = Session.Query<Model.Genre>().Single(g => g.Name == name);

			Assert.AreEqual(createdGenre.Id, result.GenreId);
		}

		// TODO - cannot create duplicate genres
	}

	[TestFixture]
	public class GenreCreatedModelTests
	{
		[Test]
		public void CanCreate()
		{
			new GenreCreatedModel("abc");
		}

		[Test]
		public void ShouldConstruct()
		{
			string id = "abc/123";
			var model = new GenreCreatedModel(id);

			Assert.AreEqual(id, model.GenreId);
		}
	}

	public class GenreCreatedModel
	{
		public GenreCreatedModel(string genreId)
		{
			GenreId = genreId;
		}

		public String GenreId { get; private set; }
	}

	[TestFixture]
	public class CreateGenreInputModelTests
	{
		[Test]
		public void CanCreate()
		{
			new CreateGenreInputModel();
		}
	}

	public class CreateGenreInputModel
	{
		public String Name { get; set; }
	}

	public class CreateEndpoint
	{
		private readonly IDocumentSession _session;

		public CreateEndpoint(IDocumentSession session)
		{
			_session = session;
		}

		public GenreCreatedModel Post(CreateGenreInputModel model)
		{
			// TODO - offload to a service
			var genre = new Model.Genre(model.Name);
			_session.Store(genre);

			return new GenreCreatedModel(genre.Id);
		}
	}
}
