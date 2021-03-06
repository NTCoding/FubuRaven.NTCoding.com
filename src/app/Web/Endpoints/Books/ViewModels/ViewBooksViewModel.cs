﻿using System;
using System.Collections.Generic;
using System.Linq;
using Model;

namespace Web.Endpoints.Books.ViewModels
{
	public class ViewBooksViewModel
	{
		public ViewBooksViewModel(IEnumerable<BookListView> books, 
			IDictionary<string, string> genres, string selectedGenre, IEnumerable<BookListView> wishlistBooks)
		{
			Books = books.OrderByDescending(b => b.Rating);
			Genres = genres.OrderBy(g => g.Value).ToDictionary(x => x.Key, x => x.Value);
			WishlistBooks = wishlistBooks;

			if (!string.IsNullOrWhiteSpace(selectedGenre) && genres.Keys.Contains(selectedGenre))
			{
				SelectedGenre = genres[selectedGenre];
			}
		}

		public IEnumerable<BookListView> Books { get; set; }

		public IDictionary<string, string> Genres { get; set; }

		public String SelectedGenre { get; set; }

		public String DefaultGenreText { get { return "-- All --"; } }

		public IEnumerable<BookListView> WishlistBooks { get; set; }

		
	}
}