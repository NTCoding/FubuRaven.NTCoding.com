using System.Linq;
using Model;
using Raven.Client;
using Web.Endpoints.SiteManagement.Book.LinkModels;
using Web.Endpoints.SiteManagement.Book.ViewModels;

namespace Web.Endpoints.SiteManagement.Book
{
	public class ViewEndpoint
	{
		private IDocumentSession _session;

		public ViewEndpoint()
		{
		}

		public ViewBookViewModel Get(ViewBookLinkModel model)
		{
			// TODO - that's actually a load
			var book = _session.Query<Model.Book>().Single(x => x.Id == model.Id);

			return new ViewBookViewModel(book);
		}
	}
}