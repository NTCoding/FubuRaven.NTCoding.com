using System;
using System.Collections.Generic;
using System.Linq;
using Web.Utilities;

namespace Web.Endpoints.SiteManagement.Book.CreateModels
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