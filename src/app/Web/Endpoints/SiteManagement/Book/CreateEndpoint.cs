using System;
using System.IO;
using System.Linq;
using FubuMVC.Core.Continuations;
using Model;
using Model.Services;
using Raven.Client;
using Web.Endpoints.SiteManagement.Book.CreateModels;
using Web.Endpoints.SiteManagement.Book.View;
using Web.Infrastructure.Services;

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
			// TODO - where are we going to store these kind of queries?
			var genres = _session
				.Query<Model.Genre>()
				.OrderBy(g => g.Name)
				.ToDictionary(g => g.Id, g => g.Name);

			return new CreateBookViewModel(genres);
		}

		public FubuContinuation Post(CreateBookInputModel model)
		{
			_bookCreater.Create(model.Title, model.Authors, model.Description_BigText, model.Genre,
			                   FileUploader.GetBytes(model.Image), model.BookStatus);

			return FubuContinuation.RedirectTo<ViewEndpoint>(e => e.Get(new ViewBookLinkModel()));
		}
	}
}