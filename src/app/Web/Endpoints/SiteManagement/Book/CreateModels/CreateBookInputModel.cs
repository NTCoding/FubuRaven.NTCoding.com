﻿using System;
using System.Collections.Generic;
using System.Web;
using Model;
using Web.Utilities;

namespace Web.Endpoints.SiteManagement.Book.CreateModels
{
	public class CreateBookInputModel
	{
		public String Title { get; set; }

		public String Genre { get; set; }

		public String Description_BigText { get; set; }

		public BookStatus BookStatus { get; set; }

		public IList<StringWrapper> Authors { get; set; }

		public HttpPostedFileBase Image { get; set; }
	}
}