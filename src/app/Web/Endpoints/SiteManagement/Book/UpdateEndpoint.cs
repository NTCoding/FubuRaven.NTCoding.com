using System.Linq;
using AutoMapper;
using Model;
using Model.Services;
using Model.Services.dtos;
using Raven.Client;
using Web.Endpoints.SiteManagement.Book.InputModels;
using Web.Endpoints.SiteManagement.Book.LinkModels;
using Web.Endpoints.SiteManagement.Book.ViewModels;
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

		public ViewBookLinkModel Post(UpdateBookInputModel model)
		{
			var dto = new UpdateBookDto();
			
			// TODO - encapsulate wrapper in an interface - pass mapping off to that?
			Mapper.DynamicMap(model, dto);
			dto.Authors     = model.Authors.ToStrings();
			dto.Description = model.Description_BigText;
			dto.Status      = model.BookStatus;
			
			updater.Update(dto);

			return new ViewBookLinkModel {Id = model.Id};
		}
	}
}