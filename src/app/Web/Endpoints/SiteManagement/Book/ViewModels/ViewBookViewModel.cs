using System;
using System.Collections.Generic;
using System.Linq;

namespace Web.Endpoints.SiteManagement.Book.ViewModels
{
	public class ViewBookViewModel
	{
		public ViewBookViewModel(Model.Book book)
		{
			Title       = book.Title;
			GenreName   = book.Genre.Name;
			Description = book.Description;
			Status      = book.Status.ToString();
			Authors     = book.Authors.ToList();
		}

		public String Title { get; private set; }

		public String GenreName { get; private set; }

		public String Description { get; private set; }

		public String Status { get; private set; }

		public IEnumerable<String> Authors { get; private set; }
	}
}