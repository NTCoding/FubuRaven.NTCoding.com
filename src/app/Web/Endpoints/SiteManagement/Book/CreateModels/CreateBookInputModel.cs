using System;
using System.Collections.Generic;
using Model;

namespace Web.Endpoints.SiteManagement.Book.CreateModels
{
	public class CreateBookInputModel
	{
		public string Title { get; set; }

		public String Genre { get; set; }

		public string Description_BigText { get; set; }

		public BookStatus BookStatus { get; set; }

		public IEnumerable<String> Authors { get; set; }

		// TODO - test a file upload?
		public object Image { get; set; }
	}
}