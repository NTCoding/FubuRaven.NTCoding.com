using System;

namespace Web.Endpoints.SiteManagement.Book.InputModels
{
	public class UpdateBookInputModel : CreateBookInputModel
	{
		public string Id { get; set; }

		public int Rating { get; set; }
	}
}