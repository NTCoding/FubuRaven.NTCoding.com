using System;
using System.Linq;
using AutoMapper;
using Web.Endpoints.SiteManagement.Book.InputModels;
using Web.Utilities;

namespace Web.Endpoints.SiteManagement.Book.ViewModels
{
	public class UpdateBookViewModel : UpdateBookInputModel
	{
		public UpdateBookViewModel(Model.Book book)
		{
			this.Authors    = book.Authors.ToStringWrappers().ToList();
			this.BookStatus = book.Status;
			this.Genre      = book.Genre.Id;
			this.GenreName  = book.Genre.Name;
			this.Id         = book.Id;
			this.Title      = book.Title;
			this.Description_BigText = book.Description;
		}

		public String GenreName { get; private set; }
	}
}