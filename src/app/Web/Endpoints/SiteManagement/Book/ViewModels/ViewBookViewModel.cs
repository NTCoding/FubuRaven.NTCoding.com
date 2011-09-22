using System;

namespace Web.Endpoints.SiteManagement.Book.ViewModels
{
	public class ViewBookViewModel
	{
		public ViewBookViewModel(Model.Book book)
		{
			// TODO - consider AutoMapper
			Title = book.Title;
		}

		public String Title { get; private set; }
	}
}