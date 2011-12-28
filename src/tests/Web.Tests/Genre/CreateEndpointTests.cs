using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Web.Endpoints.SiteManagement.Genre;
using Web.Endpoints.SiteManagement.Genre.CreateGenreModels;

namespace Web.Tests.Genre
{
	[TestFixture]
	public class CreateEndpointTests 
	{
		private CreateEndpoint _endpoint;
		private IGenreCreater creater;

		[SetUp]
		public void CanCreate()
		{
			_endpoint = new CreateEndpoint();
		}

		[Test]
		public void Post_GivenGenreName_ShouldAskCreater_ToCreateGenre()
		{
			string name = "Wickedd!!!";
			
			_endpoint.Post(new CreateGenreInputModel {Name = name});

			creater.ShouldHaveCreatedGenreWith(name);
		}

		[Test]
		public void Post_GivenGenreName_ShouldReturnCreatedGenresID()
		{
			string name = "moomin";
			var result =_endpoint.Post(new CreateGenreInputModel {Name = name});

			var createdGenre = new object();

			Assert.Fail();
		}

		// TODO - cannot create duplicate genres
	}

	public interface IGenreCreater
	{
	}

	public static class IGenreCreaterTestExtensions
	{
		public static void ShouldHaveCreatedGenreWith(this IGenreCreater creater, string name)
		{
			Assert.Fail();
		}
	}
}
