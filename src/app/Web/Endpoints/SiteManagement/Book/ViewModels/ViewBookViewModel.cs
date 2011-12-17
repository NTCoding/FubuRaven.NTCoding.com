using System;
using System.Collections.Generic;
using System.Linq;
using Web.Utilities;

namespace Web.Endpoints.SiteManagement.Book.ViewModels
{
	public class ViewBookViewModel
	{
		public ViewBookViewModel(Model.Book book)
		{
			Id = book.Id;
			Title       = book.Title;
			GenreName   = book.Genre.Name;
			Description_Html = book.Review;
			Status      = book.Status.ToString();
			Authors     = book.Authors.ToList();
			Image       = new ImageDisplayModel(book.Id) 
							{ Height = GetImageHeight(), Width = GetImageWidth() };
			this.Rating = book.Rating;
		}

		protected virtual int GetImageWidth()
		{
			return 300;
		}

		protected virtual int GetImageHeight()
		{
			return 300;
		}

		public String Id { get; set; }

		public String Title { get; private set; }

		public String GenreName { get; private set; }

		public String Description_Html { get; private set; }

		public String Status { get; private set; }

		public IEnumerable<String> Authors { get; private set; }

		public ImageDisplayModel Image { get; private set; }

		public int Rating { get; set; }
	}
}