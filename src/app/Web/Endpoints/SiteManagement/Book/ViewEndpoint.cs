using System.Linq;
using Model;
using Model.Services;
using Raven.Client;
using Web.Endpoints.SiteManagement.Book.LinkModels;
using Web.Endpoints.SiteManagement.Book.ViewModels;

namespace Web.Endpoints.SiteManagement.Book
{
	public class ViewEndpoint
	{
		private readonly IBookRetriever retriever;

		public ViewEndpoint(IBookRetriever retriever)
		{
			this.retriever = retriever;
		}

		public ViewBookViewModel Get(ViewBookLinkModel model)
		{
			var book = retriever.GetById(model.Id);

			return new ViewBookViewModel(book);
		}
	}
}