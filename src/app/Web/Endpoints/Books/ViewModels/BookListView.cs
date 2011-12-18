using System;
using Model;
using Web.Utilities;

namespace Web.Endpoints.Books.ViewModels
{
	public class BookListView
	{
		public BookListView(Book book)
		{
			this.Id = book.Id;
			this.Image = new ImageDisplayModel(book.Id) {Height = 100, Width = 100};
			this.Title = book.Title;
			this.Rating = book.Rating;
		}

		public String Id { get; set; }

		public ImageDisplayModel Image { get; set; }

		public String Title { get; set; }

		public int Rating { get; set; }
	}
}