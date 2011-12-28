using System.Linq;
using Model.Services;
using Raven.Client;
using Web.Endpoints.SiteManagement.Book.LinkModels;
using Web.Endpoints.SiteManagement.Book.ViewModels;

namespace Web.Endpoints.SiteManagement.Book
{
	public class BookEndpoint
	{
		private readonly IBookRetriever retriever;

		public BookEndpoint(IBookRetriever retriever)
		{
			this.retriever = retriever;
		}

		public BookListModel Get(BooksLinkModel booksLinkModel)
		{
			// TODO - consider paging
			var books = retriever.GetAll();

			return new BookListModel(books);
		}
	}
}