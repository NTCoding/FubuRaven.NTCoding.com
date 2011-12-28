using System;
using System.Collections.Generic;
using System.Linq;
using Model.Services;
using Web.Endpoints.SiteManagement.Book.InputModels;

namespace Web.Endpoints.SiteManagement.Book.ViewModels
{
	public class CreateBookViewModel : CreateBookInputModel
	{
		public CreateBookViewModel(IDictionary<string, string> genres)
		{
			Genres = genres.OrderBy(g => g.Value).ToDictionary(g => g.Key, g => g.Value);
		}

		public IDictionary<String, String> Genres { get; private set; }
	}
}