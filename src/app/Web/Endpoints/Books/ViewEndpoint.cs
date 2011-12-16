using Model;
using Raven.Client;
using Web.Endpoints.Books.LinkModels;
using Web.Endpoints.Books.ViewModels;

namespace Web.Endpoints.Books
{
	public class ViewEndpoint
	{
		private IDocumentSession session;

		public ViewEndpoint(IDocumentSession session)
		{
			this.session = session;
		}

		public ViewBookViewModel Get(ViewBookLinkModel linkModel)
		{
			var book = session.Load<Book>(linkModel.Id);
			
			return new ViewBookViewModel(book);
		}
	}
}