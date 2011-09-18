using System;
using System.Activities.Statements;
using System.Linq;
using System.Text;
using System.Web;
using Microsoft.Isam.Esent.Interop;
using Model;
using Model.Services;
using NUnit.Framework;
using Web.Endpoints.SiteManagement.Book;
using Web.Endpoints.SiteManagement.Book.CreateModels;
using Web.Endpoints.SiteManagement.Book.View;
using Genre = Model.Genre;

namespace Web.Tests.Books
{
	[TestFixture]
	public class CreateEndpointTests : RavenTestsBase
	{
		private CreateBookInputModel GetTestCreateBookInputModel(Model.Genre genre)
		{
			return new CreateBookInputModel
			{
				Title = "Amazing Book",
				Genre = genre.Id,
				Description_BigText = "A splendid read",
				BookStatus = BookStatus.Reviewed,
				Authors = new[] { "Jimmy Bogard", "Jimmy Slim" },
				Image = new[] { (byte)1 }
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
			_endpoint = new CreateEndpoint(new BookCreater(Session), Session);
		}

		[Test]
		public void Get_ShouldBeAccessbileFromCreateBookLinkModel()
		{
			_endpoint.Get(new CreateBookLinkModel());
		}

		[Test]
		public void Get_ViewModelShouldContainAllGenres()
		{
			var g1 = new Model.Genre("good") {Id = "1"};
			var g2 = new Model.Genre("bad") {Id = "2"};
			var g3 = new Model.Genre("ugly") {Id = "3"};

			Session.Store(g1);
			Session.Store(g2);
			Session.Store(g3);
			Session.SaveChanges();

			var viewModel = _endpoint.Get(new CreateBookLinkModel());

			Assert.IsTrue(viewModel.Genres.Any(g => g.Key == g1.Id && g.Value == g1.Name));
			Assert.IsTrue(viewModel.Genres.Any(g => g.Key == g2.Id && g.Value == g2.Name));
			Assert.IsTrue(viewModel.Genres.Any(g => g.Key == g3.Id && g.Value == g3.Name));
		}

		[Test]
		public void Post_GivenValidBookDetails_ShouldCreateBook()
		{
			var genre = GetGenreFromSession();

			var model = GetTestCreateBookInputModel(genre);

			_endpoint.Post(model);
			Session.SaveChanges();

			var book = Session.Query<Book>()
				.Where(b => b.Title == model.Title)
				.Where(b => b.Genre.Name == genre.Name)
				.Where(b => b.Description == model.Description_BigText)
				.Where(b => b.Status ==  model.BookStatus)
				.Where(b => b.Authors.Any(a => a == model.Authors.ElementAt(0)))
				.First();

			Assert.IsNotNull(book);
			Assert.IsTrue(book.Authors.Any(a => a == model.Authors.ElementAt(1)));
		}


		[Test]
		public void Post_ShouldRedirectToMangementViewBook()
		{
			var genre = GetGenreFromSession();
			var model = GetTestCreateBookInputModel(genre);

			var result = _endpoint.Post(model);

			result.AssertWasRedirectedTo<ViewEndpoint>(e => e.Get(new ViewBookLinkModel()));
		}
	}
}
