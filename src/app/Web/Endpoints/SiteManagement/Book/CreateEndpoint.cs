using System.Collections.Generic;
using System.IO;
using FubuMVC.Core.Continuations;
using Model;
using Model.Services;
using Model.Services.dtos;
using Web.Endpoints.SiteManagement.Book.InputModels;
using Web.Endpoints.SiteManagement.Book.LinkModels;
using Web.Endpoints.SiteManagement.Book.ViewModels;
using Web.Infrastructure.Services;
using Web.Utilities;

namespace Web.Endpoints.SiteManagement.Book
{
	public class CreateEndpoint
	{
		private readonly IBookCreater bookCreater;
		private readonly IGenreRetriever genreRetriever;

		public CreateEndpoint(IBookCreater bookCreater, IGenreRetriever genreRetriever)
		{
			this.bookCreater = bookCreater;
			this.genreRetriever = genreRetriever;
		}

		public CreateBookViewModel Get(CreateBookLinkModel model)
		{
			var genres = genreRetriever.GetAllOrderedByName();

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

			// TODO - should only return the Id
			var book = bookCreater.Create(dto);

			var linkModel = new ViewBookLinkModel { Id = book.Id };

			return FubuContinuation.RedirectTo(linkModel);
		}
	}
}