using System;
using System.Collections.Generic;
using System.Web;
using Model;
using Web.Utilities;

namespace Web.Endpoints.SiteManagement.Book.CreateModels
{
	public class CreateBookInputModel
	{
		public string Title { get; set; }

		public String Genre { get; set; }

		public string Description_BigText { get; set; }

		public BookStatus BookStatus { get; set; }

		public IList<StringWrapper> Authors { get; set; }

		// TODO - test a file upload?
		public HttpPostedFileBase Image { get; set; }
	}
}