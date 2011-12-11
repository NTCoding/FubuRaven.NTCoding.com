using System.Collections.Generic;

namespace Web.Endpoints.ViewModels
{
	public class ViewBooksViewModel
	{
		public ViewBooksViewModel(IEnumerable<BookListView> books, IDictionary<string, string> genres)
		{
			Books = books;
			Genres = genres;
		}

		public IEnumerable<BookListView> Books { get; set; }

		public IDictionary<string, string> Genres { get; set; }
	}
}