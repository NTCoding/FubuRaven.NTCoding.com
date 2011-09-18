using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Web.Endpoints.SiteManagement.Genre;
using Web.Endpoints.SiteManagement.Genre.CreateGenreModels;

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
}
