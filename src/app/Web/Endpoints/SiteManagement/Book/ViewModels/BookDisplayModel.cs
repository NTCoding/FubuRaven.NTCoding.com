using System;
using Model;

namespace Web.Endpoints.SiteManagement.Book.ViewModels
{
	public class BookDisplayModel
	{
		public BookDisplayModel(Model.Book book)
		{
			Id     = book.Id;
			Title  = book.Title;
			Status = book.Status;
		}

		public String Id { get; private set; }

		public String Title { get; private set; }

		public BookStatus Status { get; private set; }
	}
}