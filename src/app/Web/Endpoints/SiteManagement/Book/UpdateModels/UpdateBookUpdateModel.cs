using System;
using System.Collections.Generic;
using Model;

namespace Web.Endpoints.SiteManagement.Book.UpdateModels
{
	public class UpdateBookUpdateModel
	{
		public IList<string> Authors { get; set; }
		
		public String Description_BigText { get; set; }
		
		public BookStatus Status { get; set; }
		
		public String Genre { get; set; }
		
		public String Title { get; set; }

		public string Id { get; set; }
	}
}