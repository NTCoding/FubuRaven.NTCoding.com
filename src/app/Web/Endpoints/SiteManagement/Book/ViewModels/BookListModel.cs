using System;
using System.Collections.Generic;

namespace Web.Endpoints.SiteManagement.Book.ViewModels
{
	public class BookListModel
	{
		public BookListModel(Model.Book[] books)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<BookDisplayModel> Books { get; private set; }
	}

	public class BookDisplayModel
	{
		public BookDisplayModel(Model.Book book)
		{
			Id = book.Id;
		}

		public String Id { get; private set; }
	}
}