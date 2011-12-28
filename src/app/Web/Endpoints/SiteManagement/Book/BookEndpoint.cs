using System.Linq;
using Raven.Client;
using Web.Endpoints.SiteManagement.Book.LinkModels;
using Web.Endpoints.SiteManagement.Book.ViewModels;

namespace Web.Endpoints.SiteManagement.Book
{
	public class BookEndpoint
	{
		private readonly IDocumentSession session;

		public BookEndpoint()
		{
		}

		public BookListModel Get(BooksLinkModel booksLinkModel)
		{
			// TODO - consider paging
			var books = session.Query<Model.Book>();

			return new BookListModel(books);
		}
	}
}