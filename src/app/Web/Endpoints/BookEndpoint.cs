using System.Linq;
using Model;
using Model.Services;
using Raven.Client;
using Web.Endpoints.LinkModels;
using Web.Endpoints.ViewModels;

namespace Web.Endpoints
{
	public class BookEndpoint
	{
		private readonly IDocumentSession session;
		private readonly IGenreRetriever genreRetriever;

		public BookEndpoint(IDocumentSession session, IGenreRetriever genreRetriever)
		{
			this.session = session;
			this.genreRetriever = genreRetriever;
		}

		public ViewBooksViewModel Get(ViewBooksLinkModel model)
		{
			// TODO - book retriever - could in future be replaced by a read store / view cache
			var models = GetBooks(model).ToList().Select(b => new BookListView(b));

			return new ViewBooksViewModel(models, genreRetriever.GetAllOrderedByName());
		}

		private IQueryable<Book> GetBooks(ViewBooksLinkModel model)
		{
			return ShouldDefaultToAllGenres(model) 
			       	? session.Query<Book>()
			       	: Queryable.Where(session.Query<Book>(), b => b.Genre.Id == model.Genre);
		}

		private bool ShouldDefaultToAllGenres(ViewBooksLinkModel model)
		{
			return string.IsNullOrWhiteSpace(model.Genre);
		}
	}
}