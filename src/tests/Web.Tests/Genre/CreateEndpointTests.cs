using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Model.Services;
using Model.Services.dtos;
using NUnit.Framework;
using Rhino.Mocks;
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
			creater = MockRepository.GenerateMock<IGenreCreater>();
			_endpoint = new CreateEndpoint(creater);
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

			var idOfNewGenre = "genres/123";

			creater.Stub(c => c.Create(Arg<CreateGenreDto>.Is.Anything)).Return(idOfNewGenre);

			var id =_endpoint.Post(new CreateGenreInputModel {Name = name});

			Assert.AreEqual(idOfNewGenre, id);
		}

		// TODO - cannot create duplicate genres
	}

	public static class IGenreCreaterTestExtensions
	{
		public static void ShouldHaveCreatedGenreWith(this IGenreCreater creater, string name)
		{
			var dto = (CreateGenreDto) creater.GetArgumentsForCallsMadeOn(c => c.Create(Arg<CreateGenreDto>.Is.Anything))[0][0];

			Assert.AreEqual(name, dto.Name);
		}
	}
}
