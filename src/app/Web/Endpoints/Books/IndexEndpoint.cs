using System.Linq;
using Model;
using Model.Services;
using Raven.Client;
using Web.Endpoints.Books.LinkModels;
using Web.Endpoints.Books.ViewModels;

namespace Web.Endpoints.Books
{
	public class IndexEndpoint
	{
		private readonly IDocumentSession session;
		private readonly IGenreRetriever genreRetriever;

		public IndexEndpoint(IDocumentSession session, IGenreRetriever genreRetriever)
		{
			this.session = session;
			this.genreRetriever = genreRetriever;
		}

		public ViewBooksViewModel Get(ViewBooksLinkModel model)
		{
			// TODO - book retriever - could in future be replaced by a read store / view cache
			var models = GetBooks(model).ToList().Select(b => new BookListView(b));

			return new ViewBooksViewModel(models, genreRetriever.GetAllOrderedByName(), model.Genre);
		}

		private IQueryable<Book> GetBooks(ViewBooksLinkModel model)
		{
			var shouldDefaultToAllGenres = ShouldDefaultToAllGenres(model);

			return shouldDefaultToAllGenres 
			       	? session.Query<Book>().Where(b => b.Status == BookStatus.Reviewed)
			       	: session.Query<Book>().Where(b => b.Status == BookStatus.Reviewed && b.Genre.Id == model.Genre);
		}

		private bool ShouldDefaultToAllGenres(ViewBooksLinkModel model)
		{
			return string.IsNullOrWhiteSpace(model.Genre)
			       || !genreRetriever.CanFindGenreWith(model.Genre);
		}
	}
}