using System.Linq;
using AutoMapper;
using FubuMVC.Core.Continuations;
using Model;
using Model.Services;
using Model.Services.dtos;
using Raven.Client;
using Web.Endpoints.SiteManagement.Book.InputModels;
using Web.Endpoints.SiteManagement.Book.LinkModels;
using Web.Endpoints.SiteManagement.Book.ViewModels;
using Web.Infrastructure.Services;
using Web.Utilities;

namespace Web.Endpoints.SiteManagement.Book
{
	public class UpdateEndpoint
	{
		private readonly IBookUpdater updater;
		private readonly IGenreRetriever genreRetriever;
		private readonly IBookRetriever bookRetriever;

		public UpdateEndpoint(IBookUpdater updater, IGenreRetriever genreRetriever, IBookRetriever bookRetriever)
		{
			this.genreRetriever = genreRetriever;
			this.bookRetriever = bookRetriever;
			this.updater = updater;
		}

		public UpdateBookViewModel Get(UpdateBookLinkModel model)
		{
			// TODO - should controllers even see the domain entities? NO
			var book = bookRetriever.GetById(model.Id);
			var genres = genreRetriever.GetAll();

			return new UpdateBookViewModel(book, genres);
		}

		public FubuContinuation Post(UpdateBookInputModel model)
		{
			var dto = new UpdateBookDto
			          	{
			          		Authors = model.Authors.ToStrings(),
			          		Status  = model.BookStatus,
			          		Image   = model.Image == null || model.Image.ContentLength == 0 ? null : FileUploader.GetBytes(model.Image),
							Genre   = model.Genre,
							Id      = model.Id,
							Title   = model.Title,
							Rating  = model.Rating,
			          		Description = model.Description_BigText,
			          	};
			
			updater.Update(dto);

			return FubuContinuation.RedirectTo(new ViewBookLinkModel {Id = model.Id});
		}
	}
}