using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Model;
using Model.Services;
using Model.Services.dtos;
using NUnit.Framework;
using Rhino.Mocks;
using Web.Endpoints.SiteManagement.Book;
using Web.Endpoints.SiteManagement.Book.InputModels;
using Web.Endpoints.SiteManagement.Book.LinkModels;
using Web.Infrastructure.Services;
using Web.Tests.TestDoubles;
using Web.Tests.Utilities;
using Web.Utilities;

namespace Web.Tests.Books.SiteManagement
{
	[TestFixture]
	public class CreateEndpointTests 
	{
		private CreateBookInputModel GetTestCreateBookInputModel(Model.Genre genre)
		{
			return new CreateBookInputModel
			{
				Title               = "Amazing Book",
				Genre               = genre.Id,
				Description_BigText = "A splendid read",
				BookStatus          = BookStatus.Reviewed,
				Authors             = new[] { "Jimmy Bogard", "Jimmy Slim" }.ToStringWrappers().ToList<StringWrapper>(),
				Image               = new MockHttpPostedFileBase(new byte[100])
			};
		}

		private Model.Genre GetRandomGenre()
		{
			var genre = new Model.Genre("wooo") { Id = "1" };
			
			return genre;
		}

		private CreateEndpoint _endpoint;
		private IGenreRetriever retriever;
		private IBookCreater creater;

		[SetUp]
		public void SetUp()
		{
			creater = MockRepository.GenerateMock<IBookCreater>();
			retriever = MockRepository.GenerateMock<IGenreRetriever>();
			_endpoint = new CreateEndpoint(creater, retriever);
		}

		// TODO - This test looks pointless but is our contract for identifying this behaviour chain so must be kept
		[Test]
		public void Get_ShouldBeAccessbileFromCreateBookLinkModel()
		{
			retriever.Stub(r => r.GetAll()).Return(new Dictionary<string, string>());

			_endpoint.Get(new CreateBookLinkModel());
		}

		[Test]
		public void Get_ViewModelShouldContainAllGenres_OrderedByName()
		{
			var genres = GenreTestingHelper.Get3RandomGenres();
			retriever.Stub(r => r.GetAll()).Return(genres.ToDictionary(x => x.Id, x => x.Name));

			var viewModel = _endpoint.Get(new CreateBookLinkModel());

			genres.OrderBy(g => g.Name).ShouldMatch(viewModel.Genres);
		}

		[Test]
		public void Post_GivenValidBookDetails_ShouldCreateBook()
		{
			creater.Stub(c => c.Create(Arg<CreateBookDto>.Is.Anything)).Return("blah");

			var genre = GetRandomGenre();

			var model = GetTestCreateBookInputModel(genre);

			_endpoint.Post(model);

			creater.ShouldHaveBeenAskedToCreateFrom(model);
		}

		// TODO - this overload just cannot be called. I am going to try and fix it
		[Test][Ignore]
		public void Post_ShouldRedirect_WithIDOfCreatedBook()
		{
			var genre = GetRandomGenre();
			var model = GetTestCreateBookInputModel(genre);

			var result = _endpoint.Post(model);

			Assert.Inconclusive();
			//Func<ViewBookLinkModel, bool> predicate = x => true;
			//result.AssertWasRedirectedTo<ViewBookLinkModel>((Func<ViewBookLinkModel, bool>)predicate);
		}
	}

	// TODO - Move this class somewhere else
	public static class IBookCreaterTestExtensions
	{
		public static void ShouldHaveBeenAskedToCreateFrom(this IBookCreater creater, CreateBookInputModel model)
		{
			var dto = (CreateBookDto) creater.GetArgumentsForCallsMadeOn(c => c.Create(Arg<CreateBookDto>.Is.Anything))[0][0];

			Assert.AreEqual(dto.Authors.ToList(), model.Authors.ToStrings().ToList());
			Assert.AreEqual(dto.Description, model.Description_BigText);
			Assert.AreEqual(dto.Genre, model.Genre);
			Assert.AreEqual(dto.Image, FileUploader.GetBytes(model.Image));
			Assert.AreEqual(dto.Status, model.BookStatus);
			Assert.AreEqual(dto.Title, model.Title);
		}
	}
}
