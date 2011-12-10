using System.Collections.Generic;
using Web.Endpoints.SiteManagement.Book.InputModels;

namespace Web.Endpoints.SiteManagement.Book.ViewModels
{
	public class CreateBookViewModel : CreateBookInputModel
	{
		public CreateBookViewModel(IDictionary<string, string> genres)
		{
			Genres = genres;
		}

		public IDictionary<string, string> Genres { get; private set; }
	}
}