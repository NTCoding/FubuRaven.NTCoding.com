using System;
using AutoMapper;
using Web.Endpoints.SiteManagement.Book.UpdateModels;

namespace Web.Endpoints.SiteManagement.Book.ViewModels
{
	public class UpdateBookViewModel : UpdateBookUpdateModel
	{
		public UpdateBookViewModel(Model.Book book)
		{
			Mapper.DynamicMap(book, this);
			if (book != null) this.Genre = book.Genre.Id;
		}

		public String Id { get; set; }

		public String GenreName { get; private set; }
	}
}