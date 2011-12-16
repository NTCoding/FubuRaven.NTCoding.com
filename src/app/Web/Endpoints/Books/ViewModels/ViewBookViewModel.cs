using System;
using Model;

namespace Web.Endpoints.Books.ViewModels
{
	public class ViewBookViewModel : Web.Endpoints.SiteManagement.Book.ViewModels.ViewBookViewModel
	{
		// TODO - Am I happy with view model inheritance?
		public ViewBookViewModel(Book book) : base(book)
		{
			
			this.Review = book.Review;
		}

		public String Review { get; set; }
	}
}