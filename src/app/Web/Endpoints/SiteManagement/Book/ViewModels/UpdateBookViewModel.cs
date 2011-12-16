using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Model;
using Web.Endpoints.SiteManagement.Book.InputModels;
using Web.Utilities;

namespace Web.Endpoints.SiteManagement.Book.ViewModels
{
	public class UpdateBookViewModel : UpdateBookInputModel
	{
		public UpdateBookViewModel(Model.Book book, IDictionary<string, string> genres)
		{
			if (book != null)
			{
				this.Authors    = book.Authors.ToStringWrappers().ToList();
				this.BookStatus = book.Status;
				this.Genre      = book.Genre.Id;
				this.Id         = book.Id;
				this.Title      = book.Title;
				this.SelectedBookStatus = book.Status;
				this.SelectedGenre = book.Genre.Name;
				this.Description_BigText = book.Review;
			}
		
			this.Genres = genres;
		}

		public String SelectedGenre { get; private set; }

		public IDictionary<String, String> Genres { get; private set; }

		public BookStatus SelectedBookStatus { get; set; }
	}
}