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
}