using System;
using Model;

namespace Web.Endpoints.ViewModels
{
	public class BookListView
	{
		public BookListView(Book book)
		{
			this.Id = book.Id;
			this.Image = book.Image;
			this.Title = book.Title;
		}

		public String Id { get; set; }

		public byte[] Image { get; set; }

		public String Title { get; set; }
	}
}