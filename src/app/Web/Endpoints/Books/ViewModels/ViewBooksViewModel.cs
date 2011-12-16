using System;
using System.Collections.Generic;

namespace Web.Endpoints.Books.ViewModels
{
	public class ViewBooksViewModel
	{
		public ViewBooksViewModel(IEnumerable<BookListView> books, IDictionary<string, string> genres, string selectedGenre)
		{
			Books = books;
			Genres = genres;
			if (!string.IsNullOrWhiteSpace(selectedGenre)) SelectedGenre = genres[selectedGenre];
		}

		public IEnumerable<BookListView> Books { get; set; }

		public IDictionary<string, string> Genres { get; set; }

		public String SelectedGenre { get; set; }
	}
}