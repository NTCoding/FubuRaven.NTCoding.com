using Model;
using Model.Services;
using Raven.Client;
using Web.Endpoints.Books.LinkModels;
using Web.Endpoints.Books.ViewModels;

namespace Web.Endpoints.Books
{
	public class ViewEndpoint
	{
		private readonly IBookRetriever retriever;

		public ViewEndpoint(IBookRetriever retriever)
		{
			this.retriever = retriever;
		}

		public ViewBookViewModel Get(ViewBookLinkModel linkModel)
		{
			var book = retriever.GetById(linkModel.Id);
			var relatedBooks = retriever.GetReviewedBooks(book.Genre.Id);
			
			return new ViewBookViewModel(book, relatedBooks);
		}
	}
}