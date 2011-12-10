﻿using System.Linq;
using AutoMapper;
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
		private readonly IDocumentSession session;
		private readonly IBookUpdater updater;

		public UpdateEndpoint(IDocumentSession session, IBookUpdater updater)
		{
			this.session = session;
			this.updater = updater;
		}

		public UpdateBookViewModel Get(UpdateBookLinkModel model)
		{
			// TODO - should controllers even see the domain entities?
			var book = session.Load<Model.Book>(model.Id);

			return new UpdateBookViewModel(book);
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