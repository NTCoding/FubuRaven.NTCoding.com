using System.Linq;
using Model;
using Model.Services;
using NUnit.Framework;
using Web.Endpoints.SiteManagement.Book;
using Web.Endpoints.SiteManagement.Book.InputModels;
using Web.Endpoints.SiteManagement.Book.LinkModels;
using Web.Tests.TestDoubles;
using Web.Tests.Utilities;
using Web.Utilities;

namespace Web.Tests.Books.SiteManagement
{
	[TestFixture]
	public class CreateEndpointTests : RavenTestsBase
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

		private Model.Genre GetGenreFromSession()
		{
			var genre = new Model.Genre("wooo") { Id = "1" };
			Session.Store(genre);
			Session.SaveChanges();
			return genre;
		}

		private CreateEndpoint _endpoint;

		[SetUp]
		public void SetUp()
		{
			base.SetUp();
			_endpoint = new CreateEndpoint(new BookCreater(Session), new RavenDbGenreRetriever(Session));
		}

		[Test]
		public void Get_ShouldBeAccessbileFromCreateBookLinkModel()
		{
			_endpoint.Get(new CreateBookLinkModel());
		}

		[Test]
		public void Get_ViewModelShouldContainAllGenres()
		{
			var genres = GenreTestingHelper.GetGenresFromSession(Session);

			var viewModel = _endpoint.Get(new CreateBookLinkModel());

			genres.OrderBy(g => g.Name).ShouldMatch(viewModel.Genres);
		}

		[Test]
		public void Get_ViewModelShouldContainGenresInAlaphabeticalOrder()
		{
			var genres = new[]
			             	{
								new Model.Genre("zzz") {Id = "1"},
								new Model.Genre("xxx") {Id = "2"},
								new Model.Genre("aaa") {Id = "3"},
								new Model.Genre("fff") {Id = "4"},
			             	};

			genres.ToList().ForEach(Session.Store);
			Session.SaveChanges();

			var result = _endpoint.Get(new CreateBookLinkModel());

			var orderedInputGenres = genres.OrderBy(g => g.Name);

			for (int i = 0; i < orderedInputGenres.Count(); i++)
			{
				var expected = orderedInputGenres.ElementAt(i).Name;
				var actual = result.Genres.ElementAt(i).Value;

				Assert.AreEqual(expected, actual);
			}
		}

		[Test]
		public void Post_GivenValidBookDetails_ShouldCreateBook()
		{
			var genre = GetGenreFromSession();

			var model = GetTestCreateBookInputModel(genre);

			_endpoint.Post(model);
			Session.SaveChanges();

			// TODO This is a query - it should live somewhere else
			var book = Session.Query<Book>()
				.Where(b => b.Title == model.Title)
				.Where(b => b.Genre.Name == genre.Name)
				.Where(b => b.Description == model.Description_BigText)
				.Where(b => b.Status == model.BookStatus)
				.ToList()
				.Where(b => b.Authors.Any(a => a == model.Authors.ElementAt(0).Text))
				.First();

			Assert.IsNotNull(book);
			Assert.IsTrue(book.Authors.Any(a => a == model.Authors.ElementAt(1)));
		}

		// TODO - this overload just cannot be called. I am going to try and fix it
		[Test][Ignore]
		public void Post_ShouldRedirect_WithIDOfCreatedBook()
		{
			var genre = GetGenreFromSession();
			var model = GetTestCreateBookInputModel(genre);

			var result = _endpoint.Post(model);
			Session.SaveChanges();

			var book = Session.Query<Book>().Single();

			Assert.Inconclusive();
			//Func<ViewBookLinkModel, bool> predicate = x => true;
			//result.AssertWasRedirectedTo<ViewBookLinkModel>((Func<ViewBookLinkModel, bool>)predicate);
		}
	}
}
