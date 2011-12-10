using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
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
				this.Authors = book.Authors.ToStringWrappers().ToList();
				this.BookStatus = book.Status;
				this.Genre = book.Genre.Id;
				this.Id = book.Id;
				this.Title = book.Title;
				this.SelectedGenre = book.Genre.Name;
				this.Description_BigText = book.Description;
			}
		
			this.Genres = genres;
		}

		public String SelectedGenre { get; private set; }

		public IDictionary<String, String> Genres { get; private set; }
	}
}