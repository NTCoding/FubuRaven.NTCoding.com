using System;
using System.IO;
using System.Linq;
using FubuMVC.Core.Continuations;
using Model;
using Model.Services;
using Raven.Client;
using Web.Endpoints.SiteManagement.Book.CreateModels;
using Web.Infrastructure.Services;
using Web.Utilities;

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
				.OrderBy(g => g.Name)
				.ToDictionary(g => g.Id, g => g.Name);

			return new CreateBookViewModel(genres);
		}

		public FubuContinuation Post(CreateBookInputModel model)
		{
			var dto = new CreateBookDto
			          	{
			          		Title       = model.Title,
			          		Authors     = model.Authors.ToStrings(),
			          		Description = model.Description_BigText,
			          		Genre       = model.Genre,
			          		Image       = FileUploader.GetBytes(model.Image),
			          		Status      = model.BookStatus
			          	};

			// TODO - convert images to png's here
			var book = _bookCreater.Create(dto);

			var linkModel = new ViewBookLinkModel { Id = book.Id };

			return FubuContinuation.RedirectTo(linkModel);
		}
	}
}