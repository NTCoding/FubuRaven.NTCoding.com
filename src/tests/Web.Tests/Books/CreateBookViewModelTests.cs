using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Web.Endpoints.SiteManagement.Book.CreateModels;

namespace Web.Tests.Books
{
	[TestFixture]
	public class CreateBookViewModelTests
	{
		[Test]
		public void CanCreate()
		{
			var genres = new Dictionary<string, string>();
			genres.Add("1", "genre1");

			new CreateBookViewModel(genres);
		}

		[Test]
		public void ShouldConstructGenres()
		{
			var genres = new Dictionary<string, string>();
			var genreID = "1";
			var genreName = "genre1";

			genres.Add(genreID, genreName);

			var model = new CreateBookViewModel(genres);

			Assert.IsTrue(model.Genres.Single().Key == genreID && model.Genres.Single().Value == genreName);
		}
	}
}