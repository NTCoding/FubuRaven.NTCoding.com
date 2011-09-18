using System.Linq;
using FubuMVC.Core.Continuations;
using Model;
using Model.Services;
using Raven.Client;
using Web.Endpoints.SiteManagement.Book.CreateModels;
using Web.Endpoints.SiteManagement.Book.View;

namespace Web.Endpoints.SiteManagement.Book
{
	public class CreateEndpoint
	{
		private readonly IBookCreater _bookCreater;
		private readonly IDocumentSession _session;

		public CreateEndpoint(IBookCreater bookCreater, IDocumentSession session)
		{
			_bookCreater = bookCreater;
			_session = session;
		}

		public CreateBookViewModel Get(CreateBookLinkModel model)
		{
			var genres = _session
				.Query<Model.Genre>()
				.ToDictionary(g => g.Id, g => g.Name);

			return new CreateBookViewModel(genres);
		}

		public FubuContinuation Post(CreateBookInputModel model)
		{
			_bookCreater.Create(model.Title, model.Authors, model.Description, model.Genre,
			                    (byte[])model.Image, model.Status);

			return FubuContinuation.RedirectTo<ViewEndpoint>(e => e.Get(new ViewBookLinkModel()));
		}
	}
}