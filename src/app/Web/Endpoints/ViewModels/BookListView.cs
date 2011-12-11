using System;
using Model;
using Web.Utilities;

namespace Web.Endpoints.ViewModels
{
	public class BookListView
	{
		public BookListView(Book book)
		{
			this.Id = book.Id;
			this.Image = new ImageDisplayModel(book.Id);
			this.Title = book.Title;
		}

		public String Id { get; set; }

		public ImageDisplayModel Image { get; set; }

		public String Title { get; set; }
	}
}