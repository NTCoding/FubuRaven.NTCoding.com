using System;
using System.Collections.Generic;
using System.Linq;

namespace Web.Endpoints.SiteManagement.Book.ViewModels
{
	public class BookListModel
	{
		public BookListModel(IEnumerable<Model.Book> books)
		{
			Books = books.Select(b => new BookDisplayModel(b));
		}

		public IEnumerable<BookDisplayModel> Books { get; private set; }
	}

	public class BookDisplayModel
	{
		public BookDisplayModel(Model.Book book)
		{
			Id = book.Id;
			Title = book.Title;
		}

		public String Id { get; private set; }

		public String Title { get; private set; }
	}
}